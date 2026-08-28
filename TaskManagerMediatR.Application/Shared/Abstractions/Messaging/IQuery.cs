using MediatR;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Shared.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
}
