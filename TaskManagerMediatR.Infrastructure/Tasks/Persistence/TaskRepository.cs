using Microsoft.EntityFrameworkCore;
using Task = TaskManagerMediatR.Domain.Models.Task;
using TaskManagerMediatR.Infrastructure.Shared.Persistence;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;

namespace TaskManagerMediatR.Infrastructure.Projects.Persistence
{
    public sealed class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerMediatRDbContext _context;
        public TaskRepository(TaskManagerMediatRDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Task>> Get(CancellationToken cancellationToken = default)
        {
            var tasks = await _context.Tasks
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return tasks;
        }

        public async Task<IReadOnlyList<Task>> GetByProjectId(Guid projectId, CancellationToken cancellationToken = default)
        {
            var tasks = await _context.Tasks
                .AsNoTracking()
                .Where(t => t.ProjectId == projectId)
                .ToListAsync(cancellationToken);

            return tasks;
        }
        public IQueryable<Task> GetFilteredByProjectId(
            Guid projectId,
            string? status,
            string? priority,
            Guid? assigneeId,
            string? search,
            string sortBy,
            string sortOrder)
        {
            var query = _context.Tasks 
                .AsNoTracking()
                .Where(t => t.ProjectId == projectId);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(t => t.Status.Value == status); 

            if (!string.IsNullOrWhiteSpace(priority))
                query = query.Where(t => t.Priority.Value == priority);

            if (assigneeId is not null)
                query = query.Where(t => t.Assignments.Any(a => a.UserId == assigneeId));

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(t => t.Title.Contains(term));
            }

             return ApplySorting(query, sortBy, sortOrder);
        }

        private static IQueryable<Task> ApplySorting(
            IQueryable<Task> query,
            string sortBy,
            string sortOrder)
        {
            var desc = !string.Equals(sortOrder, "asc", StringComparison.OrdinalIgnoreCase);

            return sortBy.ToLowerInvariant() switch
            {
                "title" => desc ? query.OrderByDescending(t => t.Title).ThenBy(t => t.Id)
                                : query.OrderBy(t => t.Title).ThenBy(t => t.Id),

                "status" => desc ? query.OrderByDescending(t => t.Status).ThenBy(t => t.Id)
                                 : query.OrderBy(t => t.Status).ThenBy(t => t.Id),

                "priority" => desc ? query.OrderByDescending(t => t.Priority).ThenBy(t => t.Id)
                                   : query.OrderBy(t => t.Priority).ThenBy(t => t.Id),

                "duedate" => desc ? query.OrderByDescending(t => t.DueDate).ThenBy(t => t.Id)
                                  : query.OrderBy(t => t.DueDate).ThenBy(t => t.Id),

                "updatedat" => desc ? query.OrderByDescending(t => t.UpdatedAt).ThenBy(t => t.Id)
                                    : query.OrderBy(t => t.UpdatedAt).ThenBy(t => t.Id),

                _ => desc ? query.OrderByDescending(t => t.CreatedAt).ThenBy(t => t.Id)
                          : query.OrderBy(t => t.CreatedAt).ThenBy(t => t.Id)
            };
        }

        public async Task<Task?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return task;
        }

        public async Task<Guid> Add(Task task, CancellationToken cancellationToken = default)
        {

            await _context.Tasks.AddAsync(task, cancellationToken);
            await _context.SaveChangesAsync();

            return task.Id;
        }

        public async Task<int> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
