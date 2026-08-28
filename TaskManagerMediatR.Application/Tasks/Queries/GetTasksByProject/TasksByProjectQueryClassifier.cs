using TaskManagerMediatR.Application.Shared;
using TaskManagerMediatR.Application.Shared.Filters;
using TaskManagerMediatR.Application.Shared.Caching;

namespace TaskManagerMediatR.Application.Tasks.Queries.GetTasksByProject
{
    public static class TasksByProjectQueryClassifier
    {
        public static CacheQueryType Classify(GetTasksByProjectQuery query)
        {
            return QueryClassifier.Classify(query, q => !string.IsNullOrWhiteSpace(q.Search), IsDefault);
        }

        private static bool IsDefault(GetTasksByProjectQuery query)
        {
            return query.Page == PageNormalization.DEFAULT_PAGE
                && query.PageSize == PageNormalization.DEFAULT_PAGE_SIZE
                && string.IsNullOrWhiteSpace(query.Status)
                && string.IsNullOrWhiteSpace(query.Priority)
                && query.AssigneeId is null
                && string.Equals(query.SortBy?.Trim(), "createdAt", StringComparison.OrdinalIgnoreCase)
                && string.Equals(query.SortOrder?.Trim(), "desc", StringComparison.OrdinalIgnoreCase);
        }
    }
}
