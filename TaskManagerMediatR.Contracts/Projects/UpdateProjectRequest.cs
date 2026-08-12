namespace TaskManagerMediatR.Contracts.Projects
{
    public sealed record UpdateProjectRequest(
        string Name,
        string Description);
}
