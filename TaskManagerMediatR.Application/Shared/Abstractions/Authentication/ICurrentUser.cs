
namespace TaskManagerMediatR.Application.Shared.Abstractions.Authentication
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        bool IsAuthenticated { get; }
        bool IsInRole(string role);
        bool HasPermission(string permission);
    }
}