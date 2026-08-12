using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Tasks.Commands.AddTagToTask
{
    public sealed record AddTagToTaskCommand(
        Guid Id,
        string Name,
        string Code,
        Guid AddingById) : ICommand;
}
