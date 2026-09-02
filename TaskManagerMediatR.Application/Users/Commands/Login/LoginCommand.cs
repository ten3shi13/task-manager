using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Users.Commands.Login
{
    public sealed record LoginCommand(
        string Email,
        string Password) : ICommand<string>;
}
