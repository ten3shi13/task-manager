using TaskManagerMediatR.Application.Shared;
using TaskManagerMediatR.Application.Shared.Filters;
using TaskManagerMediatR.Application.Shared.Caching;

namespace TaskManagerMediatR.Application.Projects.Queries.GetProjectsByUser
{
    public static class ProjectsByUserClassifier
    {
        public static CacheQueryType Classify(GetProjectsByUserQuery query)
        {
            return QueryClassifier.Classify(query, q => !string.IsNullOrWhiteSpace(q.Search), IsDefault);
        }

        private static bool IsDefault(GetProjectsByUserQuery query)
        {
            return query.Page == PageNormalization.DEFAULT_PAGE
                && query.PageSize == PageNormalization.DEFAULT_PAGE_SIZE
                && query.MemberId is null
                && string.Equals(query.SortBy?.Trim(), "createdAt", StringComparison.OrdinalIgnoreCase)
                && string.Equals(query.SortOrder?.Trim(), "desc", StringComparison.OrdinalIgnoreCase);
        }
    }
}
