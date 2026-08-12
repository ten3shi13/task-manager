using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Tasks.Commands.AddCommentToTask
{
    public sealed record AddCommentToTaskCommand(
        Guid Id,
        string Text,
        Guid AuthorId) : ICommand<Guid>;
}
