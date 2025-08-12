using Microsoft.Extensions.Logging;
using Quiz.CSharp.Api.Dtos.Question;
using Quiz.CSharp.Data.Entities;
using Quiz.CSharp.Data.Models;
using Quiz.Shared.Exceptions;

namespace Quiz.CSharp.Api.Services;

public sealed class QuestionService(
    ILogger<QuestionService> logger,
    ICSharpRepository repository,
    IMapper mapper,
    ICurrentUser currentUser) : IQuestionService
{
    public async Task<PaginatedResult<QuestionResponse>> GetQuestionsByCollectionAsync(
        int collectionId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var questions = await repository.GetQuestionsByCollectionAsync(collectionId, page, pageSize, cancellationToken);
        var responses = new List<QuestionResponse>();

        foreach (var question in questions.Items)
        {
            var response = mapper.Map<QuestionResponse>(question);

            if (currentUser.IsAuthenticated && currentUser.UserId is not null)
            {
                var previousAnswer = await repository.GetLatestAnswerAsync(
                    currentUser.UserId,
                    question.Id,
                    cancellationToken);

                if (previousAnswer is not null)
                {
                    response = response with
                    {
                        PreviousAnswer = new PreviousAnswerResponse
                        {
                            Answer = previousAnswer.Answer,
                            SubmittedAt = previousAnswer.SubmittedAt,
                            IsCorrect = previousAnswer.IsCorrect
                        }
                    };
                }
            }

            responses.Add(response);
        }

        return new PaginatedResult<QuestionResponse>(responses, questions.TotalCount, questions.Page, questions.PageSize);
    }

    public async Task<List<QuestionResponse>> GetPreviewQuestionsAsync(int collectionId, CancellationToken cancellationToken = default)
    {
        var questions = await repository.GetPreviewQuestionsAsync(collectionId, cancellationToken);
        return mapper.Map<List<QuestionResponse>>(questions);
    }

    public async Task UpdateQuestionAsync(int collectionId, int questionId, UpdateQuestionDto questionDto, CancellationToken cancellationToken)
    {
        var question = await repository.GetQuestionSingleOrDefaultAsync(questionId, cancellationToken);
        if (question is null)
            throw new CustomNotFoundException($"{nameof(Question)} not found.");

        var collection = await repository.GetCollectionSingleOrDefaultAsync(collectionId, cancellationToken);
        if(collection is null)
            throw new CustomNotFoundException($"{nameof(Collection)} not found.");

        var mappedQuestionModel = mapper.Map<UpdateQuestion>(questionDto);

        mapper.Map(mappedQuestionModel, question);
        question.UpdatedAt = DateTime.UtcNow;    
        await repository.UpdateQuestionAsync(question, cancellationToken);
    }

} 