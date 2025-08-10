namespace Quiz.CSharp.Api.Services;

using AutoMapper;
using Quiz.CSharp.Api.Contracts;
using Quiz.CSharp.Data.Services;
using Quiz.Shared.Authentication;

public sealed class ProgressService(
    ICSharpRepository repository,
    IMapper mapper,
    ICurrentUser currentUser) : IProgressService
{
    public async Task<List<UserProgressResponse>> GetUserProgressAsync(CancellationToken cancellationToken = default)
    {
        var responses = new List<UserProgressResponse>();
        if (currentUser.IsAuthenticated && currentUser.UserId is not null)
        {
            var CollectionIds = await repository.GetAnsweredCollectionIdsByUserIdAsync(currentUser.UserId, cancellationToken);
            foreach (var collectionId in CollectionIds)
            {
                var userProgress = await repository.GetUserProgressAsync(currentUser.UserId, collectionId, cancellationToken);
                if (userProgress is not null)
                {
                    responses.Add(mapper.Map<UserProgressResponse>(userProgress));
                }
            }
        }
        return responses;
    }
} 