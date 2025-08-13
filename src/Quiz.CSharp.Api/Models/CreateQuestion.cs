namespace Quiz.CSharp.Api.Models;

public sealed record CreateQuestion
{
    public required int Id { get; init; }
    public required string Type { get; init; }
    public required QuestionMetadata Metadata { get; init; }
    public required QuestionContent Content { get; init; }
    public IReadOnlyList<MCQOption>? Options { get; init; }
    public IReadOnlyList<string>? Hints { get; init; }
    public string? Explanation { get; init; }
    public PreviousAnswer? PreviousAnswer { get; init; }
}

public sealed record QuestionMetadata
{
    public required int CollectionId { get; init; }
    public required string CollectionCode { get; init; }
    public required string Subcategory { get; init; }
    public required string Difficulty { get; init; }
    public int EstimatedTime { get; init; }
}

public sealed record QuestionContent
{
    public required string Prompt { get; init; }
    public string? CodeBefore { get; init; }
    public string? CodeAfter { get; init; }
    public string? CodeWithBlank { get; init; }
    public string? CodeWithError { get; init; }
    public string? Snippet { get; init; }
    public IReadOnlyList<string>? Examples { get; init; }
    public IReadOnlyList<TestCase>? TestCases { get; init; }
}

public sealed record MCQOption
{
    public required string Id { get; init; }
    public required string Option { get; init; }
}

public sealed record TestCase
{
    public required string Input { get; init; }
    public required string ExpectedOutput { get; init; }
}

public sealed record PreviousAnswer
{
    public required string Answer { get; init; }
    public required DateTime SubmittedAt { get; init; }
    public required bool IsCorrect { get; init; }
} 