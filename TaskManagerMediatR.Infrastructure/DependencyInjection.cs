using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Authentication;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Infrastructure.Projects.Persistence;
using TaskManagerMediatR.Infrastructure.Shared.Persistence;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Authentication;
using TaskManagerMediatR.Infrastructure.Users.Persistence;

namespace TaskManagerMediatR.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TaskManagerMediatRDbContext>(
                options =>
                {
                    options.UseNpgsql(configuration.GetConnectionString(nameof(TaskManagerMediatRDbContext)));
                });

            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<TaskManagerMediatRDbContext>());

            services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

            return services;
        } 
    }
}
