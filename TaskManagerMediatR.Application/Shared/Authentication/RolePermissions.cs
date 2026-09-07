using TaskManagerMediatR.Domain.Models;

namespace TaskManagerMediatR.Application.Shared.Authentication
{
    public static class RolePermissions
    {
        private static readonly Dictionary<string, string[]> Map =
            new(StringComparer.OrdinalIgnoreCase)
            {
                [Roles.User] = [],
                [Roles.Admin] =
                [
                    Permissions.UsersRead,
                    Permissions.UsersManage,
                    Permissions.ProjectsViewAny,
                    Permissions.ProjectsDeleteAny,
                    Permissions.TasksViewAny
                ]
            };

        public static IReadOnlyList<string> ForRole(string role) =>
            Map.TryGetValue(role, out var permissions) ? permissions : [];

        public static IReadOnlyList<string> ForRoles(IEnumerable<string> roles) =>
            roles.SelectMany(ForRole).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }
}
