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

    public async Task<List<CollectionResponse>> GetCollectionsAsync(CancellationToken cancellationToken = default)
    {
        var collectionsWithCounts = await collectionRepository.GetCollectionsWithQuestionCountAsync(cancellationToken);
        var responses = new List<CollectionResponse>();

        foreach (var collectionWithCount in collectionsWithCounts)
        {
            var response = mapper.Map<CollectionResponse>(collectionWithCount.Collection);
            response = response with { TotalQuestions = collectionWithCount.QuestionCount };

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

    public Task<Result<CreateCollectionResponse>> CreateCollectionWithQuestionsAsync(
        CreateCollectionRequest request,
        CancellationToken cancellationToken = default)
    {
        var collection = mapper.Map<Collection>(request);

        if (string.IsNullOrWhiteSpace(collection.Code))
        {
            return Task.FromResult(Result<CreateCollectionResponse>.Failure("Collection code cannot be empty."));
        }

        if (collectionRepository.CollectionExistsAsync(collection.Code, cancellationToken).GetAwaiter().GetResult())
        {
            return Task.FromResult(Result<CreateCollectionResponse>.Failure("Collection with this code already exists."));
        }

        var createdCollection = collectionRepository.CreateCollectionAsync(collection, cancellationToken).GetAwaiter().GetResult();

        var response = mapper.Map<CreateCollectionResponse>(createdCollection);

        return Task.FromResult(Result<CreateCollectionResponse>.Success(response));
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