
namespace TaskManagerMediatR.Contracts.Tasks
{
    public sealed record TaskForProjectViewResponse(
        Guid Id,
        string Title,
        string Status,
        string Priority,
        DateTime? DueDate,
        int AssigneesCount,
        int CommentsCount,
        IReadOnlyList<string> Tags);
}
