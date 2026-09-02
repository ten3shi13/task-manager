namespace TaskManagerMediatR.Infrastructure.Shared.Persistence.Authentication
{
    public static class PermissionPolicies
    {
        public const string Prefix = "perm:";
        public static string ForPermission(string permission) => Prefix + permission;
    }
}
