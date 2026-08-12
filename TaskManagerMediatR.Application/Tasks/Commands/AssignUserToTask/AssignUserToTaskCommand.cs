using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Tasks.Commands.AssignUserToTask
{
    public sealed record AssignUserToTaskCommand(
        Guid TaskId,
        Guid UserId,
        Guid AssignedById) : ICommand;
}
