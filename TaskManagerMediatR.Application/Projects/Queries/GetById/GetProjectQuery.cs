using TaskManagerMediatR.Contracts.Projects;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;

namespace TaskManagerMediatR.Application.Projects.Queries.GetById
{
    public sealed record GetProjectQuery(Guid Id) : IQuery<ProjectResponse>;
}
