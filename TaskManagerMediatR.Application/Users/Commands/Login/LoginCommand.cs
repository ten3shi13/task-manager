using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Contracts.Authentication;

namespace TaskManagerMediatR.Application.Users.Commands.Login
{
    public sealed record LoginCommand(
        string Email,
        string Password) : ICommand<TokenResponse>;
}
