namespace Quiz.Shared.Common;

public abstract class BaseEntity :IHasTimestamp
{
    public DateTimeOffset CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    DateTimeOffset IHasTimestamp.UpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsActive { get; init; } = true;
} 