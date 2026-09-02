namespace TaskManagerMediatR.Contracts.Users
{
    public sealed record UserResponse(
        string FirstName,
        string Email,
        DateTime CreatedAt);
}
