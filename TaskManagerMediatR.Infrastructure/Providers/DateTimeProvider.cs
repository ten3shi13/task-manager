using TaskManagerMediatR.Application.Shared.Abstractions;

namespace TaskManagerMediatR.Infrastructure.Providers
{
    public sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
