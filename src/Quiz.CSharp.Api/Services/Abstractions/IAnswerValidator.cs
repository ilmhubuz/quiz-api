using Quiz.CSharp.Data.Entities;

namespace Quiz.CSharp.Api.Services.Abstractions;

public interface IAnswerValidator
{
    Task<bool> ValidateAnswerAsync(
        Question question,
        string userAnswer,
        CancellationToken cancellationToken = default);
} 