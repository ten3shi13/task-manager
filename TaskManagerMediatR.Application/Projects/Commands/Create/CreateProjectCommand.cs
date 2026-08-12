using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Projects.Commands.Create
{
    public sealed record CreateProjectCommand(string Name, string Description, Guid OwnerId) : ICommand<Guid>;

}
