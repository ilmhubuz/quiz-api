namespace Quiz.Shared.Common;

public abstract class BaseEntity
{
    public DateTimeOffset CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsActive { get; init; } = true;
} 