namespace TaskManagerMediatR.Contracts.Tasks
{
    public sealed record CreateTaskRequest(
        Guid ProjectId,
        string Title,
        string Description,
        string Priority,
        DateTime? DueDate = null);
}
