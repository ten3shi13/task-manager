namespace TaskManagerMediatR.Domain.Models
{
    public static class Roles
    {
        public const string User = "User";
        public const string Admin = "Admin";

        public static readonly IReadOnlyList<string> All = [User, Admin];

        public static bool IsValid(string? role) =>
            !string.IsNullOrWhiteSpace(role) &&
            All.Any(r => string.Equals(r, role, StringComparison.OrdinalIgnoreCase));

        public static string Normalize(string role) =>
            All.First(r => string.Equals(r, role, StringComparison.OrdinalIgnoreCase));
    }
}
