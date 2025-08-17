using Quiz.CSharp.Data.Data;
using Quiz.CSharp.Data.Repositories.Abstractions;

namespace Quiz.CSharp.Data.Repositories;

public sealed class CollectionRepository(ICSharpDbContext context) : ICollectionRepository
{
    public async Task<IReadOnlyList<Collection>> GetCollectionsAsync(CancellationToken cancellationToken = default)
        => await context.Collections
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ToListAsync(cancellationToken);

    public async Task<Collection> CreateCollectionAsync(Collection collection, CancellationToken cancellationToken = default)
    {
        context.Collections.Add(collection);
        await context.SaveChangesAsync(cancellationToken);
        return collection;
    }

    public async Task<IReadOnlyList<CollectionWithQuestionCount>> GetCollectionsWithQuestionCountAsync(CancellationToken cancellationToken = default)
        => await context.Collections
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .Select(c => new CollectionWithQuestionCount
            {
                Collection = c,
                QuestionCount = context.Questions.Count(q => q.CollectionId == c.Id && q.IsActive)
            })
            .ToListAsync(cancellationToken);
    public async Task<bool> CollectionExistsAsync(string code, CancellationToken cancellationToken = default)
        => await context.Collections.AnyAsync(c => c.Code == code && c.IsActive, cancellationToken);
    public async Task<List<int>> GetAnsweredCollectionIdsByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = default)
        => await context.UserAnswers
            .Where(ua => ua.UserId == userId)
            .Select(ua => ua.Question.CollectionId)
            .Distinct()
            .ToListAsync(cancellationToken);

    public async Task<List<Collection>> UpdateCollectionAsync(int id, Collection updatedCollection, CancellationToken cancellationToken = default)
    {
        var collection = await context.Collections
            .Include(c => c.Questions)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (collection == null)
            throw new KeyNotFoundException($"Collection with ID {id} not found.");

        collection.Title = updatedCollection.Title;
        collection.Description = updatedCollection.Description;

        if (updatedCollection.Questions != null && updatedCollection.Questions.Any())
        {
            collection.Questions = updatedCollection.Questions;
        }

        await context.SaveChangesAsync(cancellationToken);

        return await context.Collections
            .Include(c => c.Questions)
            .ToListAsync(cancellationToken);
    }
}

public sealed class CollectionWithQuestionCount
{
    public required Collection Collection { get; init; }
    public int QuestionCount { get; init; }
} 