namespace Quiz.CSharp.Api.Services;

using AutoMapper;
using Quiz.CSharp.Api.Contracts;
using Quiz.CSharp.Data.Services;
using Quiz.Shared.Authentication;
using Quiz.Shared.Common;
using Quiz.CSharp.Data.Entities;


public sealed class QuestionService(
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

    public async Task<Result<QuestionResponse>> CreateQuestionAsync(string Type, string Subcategory, string Difficulty, string Prompt, int EstimatedTimeMinutes, CancellationToken cancellationToken = default)
    {
        var question = mapper.Map<Question>((Type, Subcategory, Difficulty, Prompt, EstimatedTimeMinutes));


        await repository.CreateQuestionAsync(question, cancellationToken);

        var response = mapper.Map<QuestionResponse>(question);
        return Result<QuestionResponse>.Success(response);
    }

}