namespace Quiz.CSharp.Api.Services;

using AutoMapper;
using Microsoft.Extensions.Logging;
using Quiz.CSharp.Api.Contracts;
using Quiz.CSharp.Api.Contracts.Requests;
using Quiz.CSharp.Data.Entities;
using Quiz.CSharp.Data.Repositories.Abstractions;
using Quiz.Shared.Authentication;
using Quiz.Shared.Common;

public sealed class CollectionService(
    ICollectionRepository collectionRepository,
    IUserProgressRepository userProgressRepository,
    IQuestionRepository questionRepository,
    IMapper mapper,
    ICurrentUser currentUser,
    ILogger<CollectionService> logger) : ICollectionService
{
    public Task<Result<CreateCollectionResponse>> CreateCollectionWithQuestionsAsync(CreateCollectionRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<CollectionResponse>> GetCollectionsAsync(CancellationToken cancellationToken = default)
    {
        var collectionsWithCounts = await collectionRepository.GetCollectionsWithQuestionCountAsync(cancellationToken);
        var responses = new List<CollectionResponse>();

        foreach (var collectionWithCount in collectionsWithCounts)
        {
            var response = mapper.Map<CollectionResponse>(collectionWithCount.Collection);
            response = response with { TotalQuestions = collectionWithCount.QuestionCount };

            // Add user progress if authenticated
            if (currentUser.IsAuthenticated && currentUser.UserId is not null)
            {
                var userProgress = await userProgressRepository.GetUserProgressOrDefaultAsync(
                    currentUser.UserId,
                    collectionWithCount.Collection.Id,
                    cancellationToken);

                if (userProgress is not null)
                {
                    response = response with
                    {
                        UserProgress = mapper.Map<UserProgressResponse>(userProgress)
                    };
                }
            }

            responses.Add(response);
        }

        return responses;
    }

    public async Task<List<CollectionResponse>> UpdateCollectionAsync(
    int id,
    UpdateCollectionRequest nextRequest,
    CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<Collection>(nextRequest);

        var collections = await collectionRepository.UpdateCollectionAsync(id, entity, cancellationToken);

        var responses = new List<CollectionResponse>();

        foreach (var collection in collections)
        {
            var response = mapper.Map<CollectionResponse>(collection);

            if (currentUser.IsAuthenticated && currentUser.UserId is not null)
            {
                var userProgress = await userProgressRepository.GetUserProgressOrDefaultAsync(
                    currentUser.UserId,
                    collection.Id,
                    cancellationToken);

                if (userProgress is not null)
                {
                    response = response with
                    {
                        UserProgress = mapper.Map<UserProgressResponse>(userProgress)
                    };
                }
            }

            responses.Add(response);
        }

        return responses;
    }
}