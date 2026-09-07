namespace TaskManagerMediatR.Contracts.Users
{
    public sealed record UpdateUserRequest(
        string FirstName,
        string Email);
}
