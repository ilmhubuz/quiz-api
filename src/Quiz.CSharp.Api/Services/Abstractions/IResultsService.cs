namespace Quiz.CSharp.Api.Services.Abstractions;

using Quiz.CSharp.Api.Contracts.Reviews;

public interface IResultsService
{
    Task<List<QuestionReviewResponse>> GetAnswerReviewAsync(
        int collectionId,
        bool includeUnanswered = false,
        CancellationToken cancellationToken = default);
    
    Task<SessionResultsResponse> CompleteSessionAsync(
        string sessionId,
        CompleteSessionRequest request,
        CancellationToken cancellationToken = default);
} 