namespace Quiz.CSharp.Api.Contracts.Requests;

public sealed record UpdateCollectionRequest
{
    public required int Id { get; set; }
    public string? Code { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? Icon { get; init; }
    public int SortOrder { get; init; }
}