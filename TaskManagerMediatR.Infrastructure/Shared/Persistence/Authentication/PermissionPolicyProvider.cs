using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace TaskManagerMediatR.Infrastructure.Shared.Persistence.Authentication
{
    public sealed class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _default;

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
            => _default = new DefaultAuthorizationPolicyProvider(options);

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _default.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _default.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(PermissionPolicies.Prefix, StringComparison.Ordinal))
            {
                var permission = policyName[PermissionPolicies.Prefix.Length..];

                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddRequirements(new PermissionRequirement(permission))
                    .Build();

                return Task.FromResult<AuthorizationPolicy?>(policy);
            }

            return _default.GetPolicyAsync(policyName);
        }
    }
}
