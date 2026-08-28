using System.Text;
using System.Security.Cryptography;
using TaskManagerMediatR.Application.Tasks.Queries.GetTasksByProject;
using TaskManagerMediatR.Application.Projects.Queries.GetProjectsByUser;

namespace TaskManagerMediatR.Application.Shared.Caching
{
    public static class CacheKeys
    {
        private const string PREFIX = "tm:v1";

        private static string Create(string value)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));

            return Convert.ToHexString(bytes);
        }

        private static string Normalize(string? value) =>
             value?.Trim().ToLowerInvariant() ?? string.Empty;

        public static string Task(Guid taskId) => 
            $"{PREFIX}:task:{taskId}";

        public static string Project(Guid projectId) => 
            $"{PREFIX}:project:{projectId}";

        public static string UserProjectsVersion(Guid userId) =>
            $"{PREFIX}:user:{userId}:projects:ver";

        public static string UserProjects(GetProjectsByUserQuery query, long version)
        {
            var normalized = string.Join(
                ":",
                $"page={query.Page}",
                $"pageSize={query.PageSize}",
                $"search={Normalize(query.Search)}",
                $"memberId={query.MemberId?.ToString() ?? string.Empty}",
                $"sortBy={Normalize(query.SortBy)}",
                $"sortOrder={Normalize(query.SortOrder)}");

            var hash = Create(normalized);

            return $"{PREFIX}:user:{query.OwnerId}:projects:ver{version}:{hash}"; 
        }

        public static string ProjectTasksVersion(Guid projectId) =>
            $"{PREFIX}:project:{projectId}:tasks:ver";

        public static string ProjectTasks(GetTasksByProjectQuery query, long version)
        {
            var normalized = string.Join(
                ":",
                $"page={query.Page}",
                $"pageSize={query.PageSize}",
                $"status={Normalize(query.Status)}",
                $"priority={Normalize(query.Priority)}",
                $"assigneeId={query.AssigneeId?.ToString() ?? string.Empty}",
                $"search={Normalize(query.Search)}",
                $"sortBy={Normalize(query.SortBy)}",
                $"sortOrder={Normalize(query.SortOrder)}");

            var hash = Create(normalized);

            return $"{PREFIX}:project:{query.ProjectId}:tasks:ver{version}:{hash}";
        }
    }
}
