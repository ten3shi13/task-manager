using Microsoft.AspNetCore.Authorization;
using TaskManagerMediatR.Application.Shared.Authentication;

namespace TaskManagerMediatR.Infrastructure.Shared.Persistence.Authentication
{
    public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var found = context.User.Claims.Any(c =>
                c.Type == CustomClaims.Permission &&
                c.Value == requirement.Permission);

            if (found)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
