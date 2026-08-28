using MediatR;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Shared.Abstractions.Messaging
{
    public interface ICommand : IRequest<Result>;

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
}
