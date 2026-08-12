using MediatR;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Shared.Abstractions.Messaging
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
                                                        where TQuery : IQuery<TResponse>
    {
    }
}
