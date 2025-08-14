namespace Quiz.CSharp.Data.Entities;

using Quiz.Shared.Common;

public sealed class Collection : BaseEntity
{
    public string? Name { get; set; }
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Icon { get; set; }
    public int SortOrder { get; set; }
    public ICollection<Question> Questions { get; set; } = [];
    public ICollection<UserProgress> UserProgress { get; init; } = [];
} 