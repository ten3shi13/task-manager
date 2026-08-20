namespace TaskManagerMediatR.Contracts.Tasks
{
    public sealed record FilterTasksRequest
    (
        int Page = 1,
        int PageSize = 20,
        string? Status = null,
        string? Priority = null,
        Guid? AssigneeId = null,
        string? Search = null,
        string? SortBy = "createdAt",
        string? SortOrder = "desc");
}
