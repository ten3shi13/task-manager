using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Application.Shared.Filters;
using TaskManagerMediatR.Contracts.Tasks;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;
using TaskManagerMediatR.Domain.ValueObjects;

namespace TaskManagerMediatR.Application.Tasks.Queries.GetTasksByProject
{
    public sealed class GetTasksByProjectQueryHandler : IQueryHandler<GetTasksByProjectQuery, PagedList<TaskForProjectViewResponse>>
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
        public async Task<Result<PagedList<TaskForProjectViewResponse>>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
        {
            if (!await _projectRepository.Exists(request.ProjectId, cancellationToken))
                return Result.Failure<PagedList<TaskForProjectViewResponse>>(DomainErrors.Project.NotFound);

            string? status = null;
            if (request.Status is not null)
            {
                var statusResult = Status.FromValue(request.Status);
                if (statusResult.IsFailure)
                    return Result.Failure<PagedList<TaskForProjectViewResponse>>(statusResult.Errors);

                status = statusResult.Value.Value;
            }

            string? priority = null;
            if (request.Priority is not null)
            {
                var priorityResult = Priority.FromValue(request.Priority);
                if (priorityResult.IsFailure)
                    return Result.Failure<PagedList<TaskForProjectViewResponse>>(priorityResult.Errors);

                priority = priorityResult.Value.Value;
            }

            var query = _taskRepository.GetFilteredByProjectId(
                request.ProjectId,
                status,
                priority,
                request.AssigneeId,
                request.Search,
                request.SortBy ?? "createdAt",
                request.SortOrder ?? "desc");

            var tasks = query.Select(t => new TaskForProjectViewResponse(
                t.Id,
                t.Title,
                t.Status.Value,
                t.Priority.Value,
                t.DueDate,
                t.Assignments.Count,
                t.Comments.Count,
                t.Tags.Select(tag => tag.Name).ToList()));

            var page = await PagedList<TaskForProjectViewResponse>.CreateAsync(
                tasks,
                request.Page,
                request.PageSize,
                cancellationToken);


            return Result.Success(page);
        }
    }
}
