using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Projects.Commands.Delete
{
    public sealed record DeleteProjectCommand(Guid Id) : ICommand<Guid>;
}
