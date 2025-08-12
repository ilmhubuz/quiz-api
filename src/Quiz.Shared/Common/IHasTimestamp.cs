namespace Quiz.Shared.Common;

public interface IHasTimestamp
{
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
}