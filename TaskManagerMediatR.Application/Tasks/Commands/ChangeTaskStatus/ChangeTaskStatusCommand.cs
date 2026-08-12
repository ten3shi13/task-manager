using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Tasks.Commands.ChangeTaskStatus
{
    public sealed record ChangeTaskStatusCommand(
        Guid Id,
        string Status,
        Guid ChangedById) : ICommand;
}
