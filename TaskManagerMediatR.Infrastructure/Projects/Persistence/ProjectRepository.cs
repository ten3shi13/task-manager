using Microsoft.EntityFrameworkCore;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Models;
using TaskManagerMediatR.Infrastructure.Shared.Persistence;

namespace TaskManagerMediatR.Infrastructure.Projects.Persistence
{
    public sealed class ProjectRepository : IProjectRepository
    {
        private readonly TaskManagerMediatRDbContext _context;
        public ProjectRepository(TaskManagerMediatRDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Project>> Get(CancellationToken cancellationToken = default)
        {
            var projects = await _context.Projects
                .Include(m => m.Members)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return projects;
        }

        public async Task<Project?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var project = await _context.Projects
                .Include(m => m.Members)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return project;
        }

        public async Task<ProjectMember?> GetMember(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var project = await _context.Projects
                    .Include(p => p.Members)
                    .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            var member = project?.Members
                .FirstOrDefault(m => m.UserId == userId);

            return member;
        }

        public async Task<Guid> Add(Project project, CancellationToken cancellationToken = default)
        {

            await _context.Projects.AddAsync(project, cancellationToken);
            await _context.SaveChangesAsync();

            return project.Id;
        }

        public async Task<int> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Projects
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
