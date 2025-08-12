namespace Quiz.CSharp.Data.Entities;

using Quiz.Shared.Common;

public sealed class UserProgress : BaseEntity
{
    public required string UserId { get; init; }
    public string? Username { get; set; }
    public string? Name { get; set; }
    public string? TelegramUsername { get; set; }
    public required int CollectionId { get; init; }
    public int TotalQuestions { get; set; }
    public int AnsweredQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public decimal SuccessRate { get; set; }
    public DateTime LastAnsweredAt { get; set; }
    public Collection Collection { get; init; } = null!;
    public DateTimeOffset? UpdatedAt { get; set; }
} 