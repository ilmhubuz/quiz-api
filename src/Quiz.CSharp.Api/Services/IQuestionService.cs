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



    // for posting questions
    Task<Result<QuestionResponse>> CreateQuestionAsync(

        string type,
        string subcategory,
        string difficulty,
        string prompt,
        int estimatedTimeMinutes,
        CancellationToken cancellationToken = default);

    // for getting a specific question by ID
    Task<Result<QuestionResponse>> GetQuestionByIdAsync(int questionId, CancellationToken cancellationToken = default);
} 