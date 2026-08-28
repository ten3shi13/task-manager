using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TaskManagerMediatR.Application.Shared.Caching;

namespace TaskManagerMediatR.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) {

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssemblyContaining<AssemblyReference>();
            });

            services.AddSingleton<IValidateOptions<CacheOptions>, CacheOptionsValidator>();

            return services;
        }
    }
}
