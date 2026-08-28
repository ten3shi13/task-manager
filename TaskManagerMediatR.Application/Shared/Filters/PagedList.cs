using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace TaskManagerMediatR.Application.Shared.Filters
{
    public sealed class PagedList<T>
    {
        [JsonConstructor]
        private PagedList(IReadOnlyList<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public IReadOnlyList<T> Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        [JsonIgnore]
        public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
        [JsonIgnore]
        public bool HasNextPage => Page * PageSize < TotalCount;
        [JsonIgnore]
        public bool HasPreviousPage => Page > 1;

        public static async Task<PagedList<T>> CreateAsync(
            IQueryable<T> query,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            page = PageNormalization.Page(page);
            pageSize = PageNormalization.PageSize(pageSize);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<T>(items, page, pageSize, totalCount);
        }
    }
}
