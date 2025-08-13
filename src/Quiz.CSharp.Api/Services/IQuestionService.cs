using Quiz.CSharp.Data.Models;

namespace Quiz.CSharp.Api.Services;

using Quiz.CSharp.Api.Contracts;
using Quiz.Shared.Common;

public interface IQuestionService
{
    Task<PaginatedResult<QuestionResponse>> GetQuestionsByCollectionAsync(
        int collectionId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<List<QuestionResponse>> GetPreviewQuestionsAsync(int collectionId, CancellationToken cancellationToken = default);

    Task<QuestionResponse> UpdateQuestionAsync(int collectionId, int questionId, Dtos.Question.UpdateQuestion updateQuestion,
        CancellationToken cancellationToken);

    List<QuestionHint> CreateHintsFromExplanation(string? explanation);
} 