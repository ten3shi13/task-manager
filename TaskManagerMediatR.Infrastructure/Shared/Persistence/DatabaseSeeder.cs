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

        private static readonly Guid MemberUserId =
            Guid.Parse("22222222-2222-2222-2222-222222222222");

        public async System.Threading.Tasks.Task SeedAsync(
            CancellationToken cancellationToken = default)
        {
            if (await dbContext.Users.AnyAsync(cancellationToken))
            {
                return;
            }

            var firstNameAdmin = FirstName.Create("Admin");
            var firstNameMember = FirstName.Create("Member");

            var emailAdmin = Email.Create("admin@example.com");
            var emailMember = Email.Create("member@example.com");

            var userAdmin = User.Create(
                AdminUserId,
                firstNameAdmin.Value,
                emailAdmin.Value,
                "111111");

            var userMember = User.Create(
                MemberUserId,
                firstNameMember.Value,
                emailMember.Value,
                "222222");

            dbContext.Users.AddRange(userAdmin.Value, userMember.Value);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
