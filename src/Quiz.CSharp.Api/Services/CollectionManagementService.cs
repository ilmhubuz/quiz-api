namespace Quiz.CSharp.Api.Services;

using Microsoft.Extensions.Logging;
using Quiz.CSharp.Api.Contracts;
using Quiz.CSharp.Api.Contracts.Requests;
using Quiz.CSharp.Data.Entities;
using Quiz.CSharp.Data.Services;
using Quiz.CSharp.Data.ValueObjects;
using Quiz.Shared.Common;

public sealed class CollectionManagementService(
    ICSharpRepository repository,
    IMapper mapper,
    ILogger<CollectionManagementService> logger) : ICollectionManagementService
{
    public async Task<Result<CreateCollectionResponse>> CreateCollectionWithQuestionsAsync(
        CreateCollectionRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate collection doesn't already exist
            if (await repository.CollectionExistsAsync(request.Code, cancellationToken))
            {
                return Result<CreateCollectionResponse>.Failure($"Collection with code '{request.Code}' already exists");
            }

            // Create collection entity
            var collection = mapper.Map<Collection>(request);

            // Save collection first to get the ID
            var createdCollection = await repository.CreateCollectionAsync(collection, cancellationToken);

            // Create questions
            var questionsCreated = 0;
            foreach (var questionRequest in request.Questions)
            {
                var question = CreateQuestionFromRequest(questionRequest, createdCollection.Id, mapper);
                if (question != null)
                {
                    await repository.CreateQuestionAsync(question, cancellationToken);
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

            return Result<CreateCollectionResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating collection {Code}", request.Code);
            return Result<CreateCollectionResponse>.Failure("An error occurred while creating the collection");
        }
    }

    private static Question? CreateQuestionFromRequest(CreateQuestionRequest request, int collectionId, IMapper mapper)
    {
        var questionType = GetQuestionTypeFromString(request.Type);
        if (questionType == null)
            return null;

        return questionType.Value switch
        {
            QuestionType.MCQ => mapper.Map<MCQQuestion>((request, collectionId)),
            QuestionType.TrueFalse => mapper.Map<TrueFalseQuestion>((request, collectionId)),
            QuestionType.Fill => mapper.Map<FillQuestion>((request, collectionId)),
            QuestionType.ErrorSpotting => mapper.Map<ErrorSpottingQuestion>((request, collectionId)),
            QuestionType.OutputPrediction => mapper.Map<OutputPredictionQuestion>((request, collectionId)),
            QuestionType.CodeWriting => mapper.Map<CodeWritingQuestion>((request, collectionId)),
            _ => null
        };
    }

    private static QuestionType? GetQuestionTypeFromString(string typeString)
    {
        return typeString.ToLowerInvariant() switch
        {
            "mcq" => QuestionType.MCQ,
            "true_false" => QuestionType.TrueFalse,
            "fill" => QuestionType.Fill,
            "error_spotting" => QuestionType.ErrorSpotting,
            "output_prediction" => QuestionType.OutputPrediction,
            "code_writing" => QuestionType.CodeWriting,
            _ => null
        };
    }
} 