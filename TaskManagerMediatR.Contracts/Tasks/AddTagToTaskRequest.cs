namespace TaskManagerMediatR.Contracts.Tasks
{
    public sealed record AddTagToTaskRequest(
        string Name,
        string Code);
}
