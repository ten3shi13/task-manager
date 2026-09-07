using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Users.Commands.Logout
{
    public sealed record LogoutCommand(string RefreshToken) : ICommand;
}
