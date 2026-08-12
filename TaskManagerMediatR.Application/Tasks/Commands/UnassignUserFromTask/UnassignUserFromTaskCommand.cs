using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Tasks.Commands.UnassignUserFromTask
{
    public sealed record UnassignUserFromTaskCommand(
        Guid TaskId,
        Guid UserId,
        Guid UnassignedById) : ICommand;
}
