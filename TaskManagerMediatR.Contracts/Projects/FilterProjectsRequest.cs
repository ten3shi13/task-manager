namespace TaskManagerMediatR.Contracts.Projects
{
    public sealed record FilterProjectsRequest(
        int Page = 1,
        int PageSize = 20,
        string? Search = null,
        Guid? OwnerId = null,
        Guid? MemberId = null,
        string SortBy = "createdAt",
        string SortOrder = "desc");
}
