using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Users.Commands.Register
{
    public sealed record RegisterCommandHandler : ICommandHandler<RegisterCommand, Guid>
    {
        public Task<Result<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
