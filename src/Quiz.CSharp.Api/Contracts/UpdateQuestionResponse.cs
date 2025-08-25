namespace Quiz.CSharp.Api.Contracts;

public class UpdateQuestionResponse
{
    public required int Id { get; set; }
    public required int CollectionId { get; set; }
    public required string Type { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; } 
}