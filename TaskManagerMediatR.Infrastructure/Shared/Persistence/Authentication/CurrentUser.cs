using TaskManagerMediatR.Application.Shared.Abstractions.Authentication;

namespace TaskManagerMediatR.Infrastructure.Shared.Persistence.Authentication
{
    public sealed class CurrentUser : ICurrentUser
    {
        public Guid UserId { get; } =
            Guid.Parse("11111111-1111-1111-1111-111111111111");
    }
}
