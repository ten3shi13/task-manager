namespace TaskManagerMediatR.Contracts.Authentication.Register
{
    public sealed record RegisterRequest(
        string FirstName,
        string Email,
        string Password);
}
