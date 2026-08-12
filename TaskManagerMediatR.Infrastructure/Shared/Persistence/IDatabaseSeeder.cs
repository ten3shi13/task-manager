
namespace TaskManagerMediatR.Infrastructure.Shared.Persistence
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync(CancellationToken cancellationToken = default);
    }
}