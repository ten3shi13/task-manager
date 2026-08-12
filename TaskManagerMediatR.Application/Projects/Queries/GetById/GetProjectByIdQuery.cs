using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Contracts.Projects;

namespace TaskManagerMediatR.Application.Projects.Queries.GetById
{
    public sealed record GetProjectByIdQuery(Guid Id) : IQuery<ProjectResponse>;
}
