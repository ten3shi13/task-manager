namespace TaskManagerMediatR.Contracts.Tasks
{
    public sealed record TaskResponse(
        Guid Id,
        Guid ProjectId,
        string Title,
        string Description,
        string Status,
        string Priority,
        DateTime? DueDate,
        Guid CreatedById,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        IReadOnlyList<AssignmentResponse> Assignees,
        IReadOnlyList<TagResponse> Tags,
        IReadOnlyList<CommentResponse> Comments);

    public sealed record AssignmentResponse(
        Guid Id,
        Guid UserId,
        Guid AssignedBy,
        DateTime AssignedAt);

    public sealed record TagResponse(
        Guid Id,
        string Name,
        string Code);

    public sealed record CommentResponse(
        Guid Id,
        Guid AuthorId,
        string Text,
        DateTime CreatedAt,
        DateTime? EditedAt);
}
