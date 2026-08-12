using Microsoft.Extensions.DependencyInjection;

namespace TaskManagerMediatR.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) {

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssemblyContaining<AssemblyReference>();
            });

            return services;
        }
    }
}
