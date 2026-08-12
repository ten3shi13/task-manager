using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Contracts.Tasks;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Tasks.Queries.GetTask
{
    public sealed class GetTaskQueryHandler : IQueryHandler<GetTaskQuery, TaskResponse>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTaskQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public async Task<Result<TaskResponse>> Handle(GetTaskQuery request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetById(request.Id, cancellationToken);
            if (task is null)
                return Result.Failure<TaskResponse>(DomainErrors.Task.NotFound);

            var response = new TaskResponse(
                task.Id,
                task.ProjectId,
                task.Title,
                task.Description,
                task.Status.Value,
                task.Priority.Value,
                task.DueDate,
                task.CreatedById,
                task.CreatedAt,
                task.UpdatedAt,
                task.Assignments.Select(a => new AssignmentResponse(
                    a.Id, a.UserId, a.AssignedBy, a.AssignedAt)).ToList(),
                task.Tags.Select(t => new TagResponse(
                    t.Id, t.Name, t.Color.Code)).ToList(),
                task.Comments.Select(c => new CommentResponse(
                    c.Id, c.AuthorId, c.Text, c.CreatedAt, c.EditedAt)).ToList());

            return response;
        }
    }
}
