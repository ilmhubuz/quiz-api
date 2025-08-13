using Quiz.CSharp.Api.Dtos.Question;

namespace Quiz.CSharp.Api.Services;
namespace Quiz.CSharp.Api.Services.Abstractions;

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

    Task UpdateQuestionAsync(int collectionId, int questionId, UpdateQuestionDto questionDto,
        CancellationToken cancellationToken);
} 