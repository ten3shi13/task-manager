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
