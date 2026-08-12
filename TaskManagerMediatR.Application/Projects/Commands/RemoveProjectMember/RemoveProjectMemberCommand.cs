using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Projects.Commands.RemoveProjectMember
{
    public sealed record RemoveProjectMemberCommand(Guid ProjectId, Guid UserId, Guid RemovedById) : ICommand;
}
