using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagerMediatR.Infrastructure.Shared.Persistence;

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

            return services;
        } 
    }
}
