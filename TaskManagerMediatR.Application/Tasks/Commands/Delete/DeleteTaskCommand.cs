using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Tasks.Commands.Delete
{
    public sealed record DeleteTaskCommand(Guid Id) : ICommand<Guid>;
}
