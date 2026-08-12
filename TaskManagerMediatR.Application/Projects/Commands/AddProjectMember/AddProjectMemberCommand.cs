using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Projects.Commands.AddProjectMember
{
    public sealed record AddProjectMemberCommand(Guid ProjectId, Guid UserId, Guid AddedById) : ICommand;
}
