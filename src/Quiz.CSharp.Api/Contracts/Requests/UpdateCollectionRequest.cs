namespace Quiz.CSharp.Api.Contracts.Requests;

public sealed record UpdateCollectionRequest
{
    public required int Id { get; set; }
    public string? Code { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? Icon { get; init; }
    public int SortOrder { get; init; }
    public List<UpdateQuestionRequest> Questions { get; init; } = [];
}

public sealed record UpdateQuestionRequest
{
    public required int Id { get; set; }
    public string? Type { get; init; }
    public string? Subcategory { get; init; }
    public string? Difficulty { get; init; }
    public string? Prompt { get; init; }
    public int EstimatedTime { get; init; }
    public string? Metadata { get; init; }
}