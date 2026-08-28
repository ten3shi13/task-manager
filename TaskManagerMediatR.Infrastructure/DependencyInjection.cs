using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using TaskManagerMediatR.Application.Shared.Abstractions;
using TaskManagerMediatR.Application.Shared.Abstractions.Authentication;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Application.Shared.Caching;
using TaskManagerMediatR.Infrastructure.Caching;
using TaskManagerMediatR.Infrastructure.Idempotence;
using TaskManagerMediatR.Infrastructure.Projects.Persistence;
using TaskManagerMediatR.Infrastructure.Services;
using TaskManagerMediatR.Infrastructure.Shared.Persistence;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Authentication;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Interceptors;
using TaskManagerMediatR.Infrastructure.Users.Persistence;

namespace TaskManagerMediatR.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("Redis")
                ?? throw new InvalidOperationException("Connection string 'Redis' is required.");

            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(connection));

            services.AddSingleton<IDistributedCache>(sp =>
            {
                var mux = sp.GetRequiredService<IConnectionMultiplexer>();
                return new RedisCache(Options.Create(new RedisCacheOptions
                {
                    ConnectionMultiplexerFactory = () => Task.FromResult(mux),
                    InstanceName = "taskmanager:"
                }));
            });

            services.AddSingleton<ICacheService, RedisCacheService>();
            services.AddSingleton<ICacheVersionService, CacheVersionService>();
            services.AddSingleton<IRequestCoalescer, RequestCoalescer>();
            services.AddSingleton<IQueryCachePolicyFactory, QueryCachePolicyFactory>();

            services.AddScoped<ITaskCacheInvalidator, TaskCacheInvalidator>();
            services.AddScoped<IProjectCacheInvalidator, ProjectCacheInvalidator>();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

            services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

            services.AddDbContext<TaskManagerMediatRDbContext>(
                (sp, options) =>
                {
                    options.UseNpgsql(configuration.GetConnectionString(nameof(TaskManagerMediatRDbContext)))
                        .AddInterceptors(sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>());
                });

            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<TaskManagerMediatRDbContext>());

            services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

            return services;
        } 
    }
}
