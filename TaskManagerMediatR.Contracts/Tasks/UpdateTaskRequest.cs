namespace TaskManagerMediatR.Contracts.Tasks
{
    public sealed record UpdateTaskRequest(
        string Title,
        string Description,
        string Priority,
        DateTime? DueDate);
}
