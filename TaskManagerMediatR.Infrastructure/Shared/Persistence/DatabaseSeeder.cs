using Microsoft.EntityFrameworkCore;
using TaskManagerMediatR.Domain.Models;
using TaskManagerMediatR.Domain.ValueObjects;

namespace TaskManagerMediatR.Infrastructure.Shared.Persistence
{
    public sealed class DatabaseSeeder(
    TaskManagerMediatRDbContext dbContext) : IDatabaseSeeder
    {
        private static readonly Guid AdminUserId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        public async System.Threading.Tasks.Task SeedAsync(
            CancellationToken cancellationToken = default)
        {
            if (await dbContext.Users.AnyAsync(cancellationToken))
            {
                return;
            }

            var firstName = FirstName.Create("Admin");

            var email = Email.Create("admin@example.com");

            var user = User.Create(
                AdminUserId,
                firstName.Value,
                email.Value,
                "111111");

            dbContext.Users.Add(user.Value);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
