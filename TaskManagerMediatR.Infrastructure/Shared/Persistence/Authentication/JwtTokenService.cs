using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TaskManagerMediatR.Domain.Models;
using TaskManagerMediatR.Application.Shared.Authentication;
using TaskManagerMediatR.Application.Shared.Abstractions.Authentication;

namespace TaskManagerMediatR.Infrastructure.Shared.Persistence.Authentication
{
    public sealed class JwtTokenService : IJwtTokenService
    {
        private readonly JwtTokenOptions _jwtTokenOptions;
        private readonly SymmetricSecurityKey _securityKey;

        public DateTime AccessTokenExpiresAtUtc =>
            DateTime.UtcNow.AddMinutes(_jwtTokenOptions.AccessTokenExpirationMinutes);
        public JwtTokenService(IOptions<JwtTokenOptions> jwtTokenOptions)
        {
            _jwtTokenOptions = jwtTokenOptions.Value;
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.SecretKey));
        }
        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email.Value),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Role, user.Role)
            };

            foreach (var permission in RolePermissions.ForRoles([user.Role]))
                claims.Add(new Claim(CustomClaims.Permission, permission));

            var credentionals = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtTokenOptions.Issuer,
                audience: _jwtTokenOptions.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: AccessTokenExpiresAtUtc,
                signingCredentials: credentionals);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
