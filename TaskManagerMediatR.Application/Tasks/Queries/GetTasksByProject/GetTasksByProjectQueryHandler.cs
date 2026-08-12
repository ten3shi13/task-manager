using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Contracts.Tasks;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Tasks.Queries.GetTasksByProject
{
    public sealed class GetTasksByProjectQueryHandler : IQueryHandler<GetTasksByProjectQuery, IReadOnlyList<TaskForProjectViewResponse>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;

        public GetTasksByProjectQueryHandler(
            ITaskRepository taskRepository,
            IProjectRepository projectRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
        }
        public async Task<Result<IReadOnlyList<TaskForProjectViewResponse>>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetById(request.ProjectId, cancellationToken);
            if (project is null)
                return Result.Failure<IReadOnlyList<TaskForProjectViewResponse>>(DomainErrors.Project.NotFound);

            var tasks = await _taskRepository.GetByProjectId(request.ProjectId, cancellationToken);

            var response = tasks.Select(t => new TaskForProjectViewResponse(
                t.Id,
                t.Title,
                t.Status.Value,
                t.Priority.Value,
                t.DueDate,
                t.Assignments.Count,
                t.Comments.Count,
                t.Tags.Select(tag => tag.Name).ToList())).ToList().AsReadOnly();

            return response;
        }
    }
}
