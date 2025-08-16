namespace Quiz.Shared.Common;

public interface IHasTimestamp
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}