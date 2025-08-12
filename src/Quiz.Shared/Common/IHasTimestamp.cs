namespace Quiz.Shared.Common;

public interface IHasTimestamp
{
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; set; }
}