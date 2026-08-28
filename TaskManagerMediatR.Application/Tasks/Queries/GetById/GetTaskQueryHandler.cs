using Microsoft.Extensions.Options;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Messaging;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Application.Shared.Caching;
using TaskManagerMediatR.Contracts.Tasks;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Application.Tasks.Queries.GetById
{
    public sealed class GetTaskQueryHandler : IQueryHandler<GetTaskQuery, TaskResponse>
    {
        private readonly CacheOptions _options;
        private readonly ICacheService _cacheService;
        private readonly IRequestCoalescer _coalescer;
        private readonly ITaskRepository _taskRepository;

        public GetTaskQueryHandler(
            IOptions<CacheOptions> options,
            ICacheService cacheService,
            IRequestCoalescer coalescer,
            ITaskRepository taskRepository)
        {
            _options = options.Value;
            _coalescer = coalescer;
            _cacheService = cacheService;
            _taskRepository = taskRepository;
        }
        public async Task<Result<TaskResponse>> Handle(GetTaskQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = CacheKeys.Task(request.Id);

            var cached = await _cacheService.Get<TaskResponse>(cacheKey, cancellationToken);

            if (cached is not null)
            {
                //var access = await EnsureCanAccessProject(cachedTask.ProjectId, cancellationToken);
                //if (access.IsFailure)
                //    return Result.Failure<TaskResponse>(access.Errors);

                return Result.Success(cached);
            }

            var response = await _coalescer.GetOrCreateAsync(cacheKey,
                async ct =>
                {
                    var cachedTask = await _cacheService.Get<TaskResponse>(cacheKey, ct);
                    if (cachedTask is not null)
                        return cachedTask;

                    var task = await _taskRepository.GetById(request.Id, ct);
                    if (task is null)
                        return null;

                    var dto = Map(task);
                    await _cacheService.Set(cacheKey, dto, _options.TaskExpiration, ct);
                    return dto;
                },
            cancellationToken);

            if (response is null)
                return Result.Failure<TaskResponse>(DomainErrors.Task.NotFound);

            return response;
        }

        private static TaskResponse Map(Domain.Models.Task task) => new(
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
            task.Tags.Select(t => new TagResponse(t.Id, t.Name, t.Color.Code)).ToList(),
            task.Comments.Select(c => new CommentResponse(
                c.Id, c.AuthorId, c.Text, c.CreatedAt, c.EditedAt)).ToList());
    }
}
