
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Tasks.Commands.RemoveTagFromTask
{
    public sealed record RemoveTagFromTaskCommand(
        Guid Id,
        Guid TagId,
        Guid RemovedById) : ICommand;
}
