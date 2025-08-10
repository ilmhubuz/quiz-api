namespace Quiz.CSharp.Api.Services;

using Quiz.CSharp.Api.Contracts;

public interface IProgressService
{
    Task<List<UserProgressResponse>> GetUserProgressAsync(CancellationToken cancellationToken = default);
} 