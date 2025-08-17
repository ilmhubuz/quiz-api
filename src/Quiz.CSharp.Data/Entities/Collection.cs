namespace Quiz.CSharp.Data.Entities;

using Quiz.Shared.Common;

public sealed class Collection : BaseEntity
{
    public int Id { get; set; }
    public required string Code { get; init; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Icon { get; init; }
    public int SortOrder { get; init; }
    public ICollection<Question> Questions { get; set; } = [];
    public ICollection<UserProgress> UserProgress { get; init; } = [];
} 