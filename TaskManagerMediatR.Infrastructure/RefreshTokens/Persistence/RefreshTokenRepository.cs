using Microsoft.EntityFrameworkCore;
using TaskManagerMediatR.Application.Shared.Abstractions.Repositories;
using TaskManagerMediatR.Domain.Models;
using TaskManagerMediatR.Infrastructure.Shared.Persistence;
using Task = System.Threading.Tasks.Task;

namespace TaskManagerMediatR.Infrastructure.RefreshTokens.Persistence
{
    public sealed class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly TaskManagerMediatRDbContext _context;
        public RefreshTokenRepository(TaskManagerMediatRDbContext context)
        {
            _context = context;
        }
        public async Task Add(RefreshToken token, CancellationToken cancellationToken = default) =>
            await _context.RefreshTokens.AddAsync(token, cancellationToken);
       
        public async Task<IReadOnlyList<RefreshToken>> GetActiveByFamily(Guid tokenFamilyId, CancellationToken cancellationToken = default) =>
            await _context.RefreshTokens
                .Where(rt => rt.TokenFamilyId == tokenFamilyId && rt.RevokedAtUtc == null && rt.ExpiresAtUtc > DateTime.UtcNow)
                .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<RefreshToken>> GetActiveByUser(Guid userId, CancellationToken cancellationToken = default) =>
            await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.RevokedAtUtc == null && rt.ExpiresAtUtc > DateTime.UtcNow)
                .ToListAsync(cancellationToken);

        public async Task<RefreshToken?> GetByHash(string tokenHash, CancellationToken cancellationToken = default) =>
           await _context.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);
    }
}
