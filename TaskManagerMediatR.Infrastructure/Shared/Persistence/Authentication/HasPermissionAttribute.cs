using Microsoft.AspNetCore.Authorization;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Constants;

namespace TaskManagerMediatR.Infrastructure.Shared.Persistence.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permission) => 
            Policy = PermissionPolicies.ForPermission(permission);
    }
}
