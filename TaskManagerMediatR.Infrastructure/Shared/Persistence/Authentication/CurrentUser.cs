using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManagerMediatR.Application.Shared.Abstractions.Authentication;
using TaskManagerMediatR.Application.Shared.Authentication;

namespace TaskManagerMediatR.Infrastructure.Shared.Persistence.Authentication
{
    internal sealed class CurrentUser(IHttpContextAccessor httpAccessor) : ICurrentUser
    {
        private ClaimsPrincipal? Principal => httpAccessor.HttpContext?.User;

        public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;
        public Guid UserId
        {
            get
            {
                var claimId = Principal?.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? Principal?.FindFirstValue(JwtRegisteredClaimNames.Sub);

                return Guid.TryParse(claimId, out var id)
                    ? id
                    : throw new UnauthorizedAccessException("User id claim is missing.");
            }
        }

        public bool IsInRole(string role) => Principal?.IsInRole(role) == true;

        public bool HasPermission(string permission) =>
            Principal?.Claims.Any(c =>
                c.Type == CustomClaims.Permission && c.Value == permission) == true;
    }
}
