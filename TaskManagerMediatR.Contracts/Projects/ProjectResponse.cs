namespace TaskManagerMediatR.Contracts.Projects
{
    public sealed record ProjectResponse(
        Guid Id,
        string Name,
        string Description,
        DateTime CreatedAt,
        Guid OwnerId,
        IReadOnlyList<ProjectMemberResponse> Members);

    public sealed record ProjectMemberResponse(
        Guid Id,
        string Role,
        DateTime JoinedAt);
}
