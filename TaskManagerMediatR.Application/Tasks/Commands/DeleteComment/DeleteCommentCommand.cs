using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Tasks.Commands.DeleteComment
{
    public sealed record DeleteCommentCommand(
        Guid TaskId,
        Guid CommentId,
        Guid DeletedById) : ICommand;
}
