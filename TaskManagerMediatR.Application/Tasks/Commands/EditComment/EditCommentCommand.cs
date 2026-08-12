using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Tasks.Commands.EditComment
{
    public sealed record EditCommentCommand(
        Guid Id,
        Guid CommentId,
        string Text,
        Guid EditorId) : ICommand;
}
