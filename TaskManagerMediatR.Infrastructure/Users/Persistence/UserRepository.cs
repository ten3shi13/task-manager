using Microsoft.EntityFrameworkCore;
using TaskManagerMediatR.Domain.Models;
using TaskManagerMediatR.Infrastructure.Shared.Persistence;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;

namespace TaskManagerMediatR.Infrastructure.Users.Persistence
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly TaskManagerMediatRDbContext _context;
        public UserRepository(TaskManagerMediatRDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<User>> Get(CancellationToken cancellationToken = default)
        {
            return await _context.Users
                                    .AsNoTracking()
                                    .ToListAsync(cancellationToken);
        }

        public async Task<User?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.Email.Value == email, cancellationToken);
        }

        public async Task<Guid> Add(User user, CancellationToken cancellationToken = default)
        {

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<int> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
