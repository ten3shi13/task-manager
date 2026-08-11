using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TaskManagerMediatR.Domain.Models;

using Threading = System.Threading.Tasks;

namespace TaskManagerMediatR.Infrastructure.Shared.Persistence
{
    public sealed class TaskManagerMediatRDbContext(DbContextOptions<TaskManagerMediatRDbContext> options) : DbContext(options)
    {
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Domain.Models.Task> Tasks => Set<Domain.Models.Task>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskManagerMediatRDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }


    }
}
