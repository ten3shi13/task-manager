using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Projects.Commands.Update
{
    public sealed record UpdateProjectCommand(Guid ProjectId, string Name, string Description, Guid UpdatedById) : ICommand<Guid>;
}
