namespace TaskManagerMediatR.Application.Shared.Authentication
{
    public static class Permissions
    {
        public const string UsersRead = "users:read";
        public const string UsersManage = "users:manage";

        public const string ProjectsViewAny = "projects:view_any";
        public const string ProjectsDeleteAny = "projects:delete_any";

        public const string TasksViewAny = "tasks:view_any";
    }
}
