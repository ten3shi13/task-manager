using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Tasks.Commands.Update
{
    public sealed record UpdateTaskCommand(
        Guid Id,
        string Title,
        string Description,
        string Priority,
        DateTime? DueDate,
        Guid UpdatedById) : ICommand;
}
