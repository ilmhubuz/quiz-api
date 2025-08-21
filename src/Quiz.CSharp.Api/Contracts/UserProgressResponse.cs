namespace Quiz.CSharp.Api.Contracts;

public sealed record UserProgressResponse
{
    public string? Username { get; init; }
    public string? Name { get; init; }
    public string? TelegramUsername { get; init; }
    public int AnsweredQuestions { get; init; }
    public int CorrectAnswers { get; init; }
    public decimal SuccessRate { get; init; }
    public decimal CompletionRate { get; init; }
}

public sealed record UserProgressManagementResponse
{
    public string UserId { get; init; } = string.Empty;
    public string? Username { get; init; }
    public string? Name { get; init; }
    public string? TelegramUsername { get; init; }
    public int CollectionId { get; init; }
    public string CollectionCode { get; init; } = string.Empty;
    public string CollectionTitle { get; init; } = string.Empty;
    public int TotalQuestions { get; init; }
    public int AnsweredQuestions { get; init; }
    public int CorrectAnswers { get; init; }
    public decimal SuccessRate { get; init; }
    public decimal CompletionRate { get; init; }
    public DateTimeOffset LastAnsweredAt { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
}

public sealed record UserProgressGroupedResponse
{
    public string UserId { get; init; } = string.Empty;
    public string? Username { get; init; }
    public string? Name { get; init; }
    public string? TelegramUsername { get; init; }
    public DateTimeOffset FirstActivityAt { get; init; }
    public DateTimeOffset LastActivityAt { get; init; }
    public int TotalCollections { get; init; }
    public int TotalQuestionsAnswered { get; init; }
    public int TotalCorrectAnswers { get; init; }
    public decimal OverallSuccessRate { get; init; }
    public List<CollectionProgressResponse> CollectionProgresses { get; init; } = [];
}

public sealed record CollectionProgressResponse
{
    public int CollectionId { get; init; }
    public string CollectionCode { get; init; } = string.Empty;
    public string CollectionTitle { get; init; } = string.Empty;
    public string CollectionDescription { get; init; } = string.Empty;
    public int TotalQuestions { get; init; }
    public int AnsweredQuestions { get; init; }
    public int CorrectAnswers { get; init; }
    public decimal SuccessRate { get; init; }
    public decimal CompletionRate { get; init; }
    public DateTimeOffset LastAnsweredAt { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
}