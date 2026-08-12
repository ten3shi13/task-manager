namespace TaskManagerMediatR.Contracts.Tasks
{
    public sealed record ChangeTaskStatusRequest(
        string Status);
}
