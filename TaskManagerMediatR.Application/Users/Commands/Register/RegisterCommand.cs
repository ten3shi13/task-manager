using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Users.Commands.Register
{
    public sealed record RegisterCommand(
        string FirstName,
        string Email,
        string Password) : ICommand<Guid>;
}
