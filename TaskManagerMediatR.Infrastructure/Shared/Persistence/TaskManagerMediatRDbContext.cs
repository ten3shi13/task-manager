using Microsoft.EntityFrameworkCore;
using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Domain.Models;

using Threading = System.Threading.Tasks;

namespace TaskManagerMediatR.Infrastructure.Shared.Persistence
{
    public sealed class TaskManagerMediatRDbContext(DbContextOptions<TaskManagerMediatRDbContext> options) : DbContext(options), IUnitOfWork
    {
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Domain.Models.Task> Tasks => Set<Domain.Models.Task>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskManagerMediatRDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public async Threading.Task CommitChangesAsync(CancellationToken cancellationToken = default) => await base.SaveChangesAsync(cancellationToken);

    }
}
