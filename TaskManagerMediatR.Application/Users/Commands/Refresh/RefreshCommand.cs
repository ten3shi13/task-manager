using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Contracts.Authentication;

namespace TaskManagerMediatR.Application.Users.Commands.Refresh
{
    public sealed record RefreshCommand(string RefreshToken) : ICommand<TokenResponse>;
}
