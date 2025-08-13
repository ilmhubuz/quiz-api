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
    Task<Result<QuestionResponse>> CreateQuestionAsync(
        string type,
        string subcategory,
        string difficulty,
        string prompt,
        int estimatedTimeMinutes,
        CancellationToken cancellationToken = default);
}