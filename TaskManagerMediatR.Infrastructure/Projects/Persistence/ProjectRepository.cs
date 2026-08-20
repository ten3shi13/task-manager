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
        public IQueryable<Project> GetFiltered(
            string? search,
            Guid? ownerId,
            Guid? memberId,
            string sortBy,
            string sortOrder)
        {
            var query = _context.Projects.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(p =>
                    p.Name.Contains(term) ||
                    (p.Description != null && p.Description.Contains(term)));
            }

            if (ownerId is not null)
                query = query.Where(p => p.OwnerId == ownerId);

            if (memberId is not null)
                query = query.Where(p => p.Members.Any(m => m.UserId == memberId));

            return ApplySorting(query, sortBy, sortOrder);
        }

        private static IQueryable<Project> ApplySorting(
            IQueryable<Project> query,
            string sortBy,
            string sortOrder)
        {
            var desc = !string.Equals(sortOrder, "asc", StringComparison.OrdinalIgnoreCase);

            return sortBy.ToLowerInvariant() switch
            {
                "name" => desc
                    ? query.OrderByDescending(p => p.Name).ThenBy(p => p.Id)
                    : query.OrderBy(p => p.Name).ThenBy(p => p.Id),

                _ => desc
                    ? query.OrderByDescending(p => p.CreatedAt).ThenBy(p => p.Id)
                    : query.OrderBy(p => p.CreatedAt).ThenBy(p => p.Id)
            };
        }

        public async Task<Project?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var project = await _context.Projects
                .Include(m => m.Members)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return project;
        }

        public Task<bool> Exists(Guid id, CancellationToken ct = default)
            => _context.Projects.AsNoTracking().AnyAsync(p => p.Id == id, ct);

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
