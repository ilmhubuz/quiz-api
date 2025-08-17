namespace Quiz.CSharp.Api.Contracts;

public sealed record UpdateCollectionResponse
{
    public int Id { get; set; }
    public string Code { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Icon { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public DateTime CreatedAt { get; init; }
    public int TotalQuestions { get; init; }
}