using System.Security.Cryptography;
using System.Text;
using TaskManagerMediatR.Application.Shared.Abstractions.Authentication;

namespace TaskManagerMediatR.Infrastructure.Shared.Persistence.Authentication
{
    internal sealed class TokenHashService : ITokenHashService
    {
        public string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        public string Hash(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes);
        }
    }
}
