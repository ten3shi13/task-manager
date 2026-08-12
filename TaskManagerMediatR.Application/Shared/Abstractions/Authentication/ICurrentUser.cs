
namespace TaskManagerMediatR.Application.Shared.Abstractions.Authentication
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
    }
}