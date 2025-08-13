namespace Quiz.CSharp.Api.Contracts;

public sealed record CreateQuestionResponse
{
    public required string Type { get; init; }
    public required QuestionMetadata Metadata { get; init; }
    public required QuestionContent Content { get; init; }
    public IReadOnlyList<MCQOptionResponse>? Options { get; init; }
    public IReadOnlyList<string>? Hints { get; init; }
    public string? Explanation { get; init; }
    public PreviousAnswerResponse? PreviousAnswer { get; init; }
}