namespace TaskManagerMediatR.Application.Shared.Filters
{
    public static class PageNormalization
    {
        public const int DEFAULT_PAGE = 1;
        public const int DEFAULT_PAGE_SIZE = 20;
        public const int MAX_PAGE_SIZE = 100;

        public static int Page(int page) =>
            page < DEFAULT_PAGE ? DEFAULT_PAGE : page;
        public static int PageSize(int pageSize) => 
            pageSize is < 1 or > MAX_PAGE_SIZE ? DEFAULT_PAGE_SIZE : pageSize;
    }
}
