using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Tasks.Commands.Create
{
    public sealed record CreateTaskCommand(
        Guid ProjectId,
        string Title,
        string Description,
        string Priority,
        Guid CreatedById,
        DateTime? DueDate = null) : ICommand<Guid>;
}
