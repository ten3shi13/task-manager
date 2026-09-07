using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Authentication;

namespace TaskManagerMediatR.API.OptionsSetup
{
    public sealed class JwtBearerTokenOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtTokenOptions _jwtTokenOptions;

        public JwtBearerTokenOptionsSetup(IOptions<JwtTokenOptions> jwtTokenOptions)
        {
            _jwtTokenOptions = jwtTokenOptions.Value;
        }

        public void Configure(JwtBearerOptions options)
        {
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtTokenOptions.Issuer,
                ValidAudience = _jwtTokenOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.SecretKey)),
                ClockSkew = TimeSpan.FromSeconds(30),
                RoleClaimType = ClaimTypes.Role,
                NameClaimType = ClaimTypes.NameIdentifier,
            };
        }

        public void Configure(string? name, JwtBearerOptions options) => 
            Configure(options);
    }
}
