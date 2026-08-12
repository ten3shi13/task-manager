using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Contracts.Projects;

namespace TaskManagerMediatR.Application.Projects.Queries.Get
{
    public record class GetProjectsQuery : IQuery<IReadOnlyList<ProjectResponse>>;
}
