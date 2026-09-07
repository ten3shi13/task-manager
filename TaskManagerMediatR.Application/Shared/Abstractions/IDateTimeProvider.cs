namespace TaskManagerMediatR.Application.Shared.Abstractions
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
