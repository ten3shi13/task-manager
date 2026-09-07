using Microsoft.AspNetCore.Authorization;

namespace TaskManagerMediatR.Infrastructure.Shared.Persistence.Authentication
{
    public sealed class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
    }
}
