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
        try
        {
            // Validate collection doesn't already exist
            if (await collectionRepository.CollectionExistsAsync(request.Code, cancellationToken))
            {
                return Result<CreateCollectionResponse>.Failure($"Collection with code '{request.Code}' already exists");
            }

            // Create collection entity
            var collection = mapper.Map<Collection>(request);

            // Save collection first to get the ID
            var createdCollection = await collectionRepository.CreateCollectionAsync(collection, cancellationToken);

            // Create questions
            var questionsCreated = 0;
            foreach (var questionRequest in request.Questions)
            {
                var question = CreateQuestionFromRequest(questionRequest, createdCollection.Id, mapper);
                if (question != null)
                {
                    await questionRepository.CreateQuestionAsync(question, cancellationToken);
                    questionsCreated++;
                }
                else
                {
                    logger.LogWarning("Failed to create question of type {Type}", questionRequest.Type);
                }
            }

            logger.LogInformation("Created collection {Code} with {QuestionCount} questions", 
                request.Code, questionsCreated);

            var response = mapper.Map<CreateCollectionResponse>(createdCollection);
            response.QuestionsCreated = questionsCreated;

            return Result<CreateCollectionResponse>.Success(response);
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
    private static Question? CreateQuestionFromRequest(CreateQuestionRequest request, int collectionId, IMapper mapper)
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

        Question? question = questionType.Value switch
        {
            QuestionType.MCQ => mapper.Map<MCQQuestion>(request),
            QuestionType.TrueFalse=> mapper.Map<TrueFalseQuestion>(request),
            QuestionType.Fill=> mapper.Map<FillQuestion>(request),
            QuestionType.ErrorSpotting=> mapper.Map<ErrorSpottingQuestion>(request),
            QuestionType.OutputPrediction=> mapper.Map<OutputPredictionQuestion>(request),
             QuestionType.CodeWriting=> mapper.Map<CodeWritingQuestion>(request),
            _ => null
        };

        if (question is not null)
            question.CollectionId = collectionId;
        return question;
    }

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