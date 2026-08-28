using Microsoft.Extensions.Options;
using TaskManagerMediatR.Application.Shared.Abstractions.Caching;

namespace TaskManagerMediatR.Application.Shared.Caching
{
    public sealed class QueryCachePolicyFactory : IQueryCachePolicyFactory
    {
        private readonly CacheOptions _options;

        public QueryCachePolicyFactory(IOptions<CacheOptions> options)
        {
            _options = options.Value;
        }
        public IQueryCachePolicy CreateForTasks()
        {
            return new QueryCachePolicy(_options.Tasks);
        }

        public IQueryCachePolicy CreateForProjects()
        {
            return new QueryCachePolicy(_options.Projects);
        }
    }
}
