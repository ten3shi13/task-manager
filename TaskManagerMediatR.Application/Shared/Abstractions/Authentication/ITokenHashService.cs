namespace TaskManagerMediatR.Application.Shared.Abstractions.Authentication
{
    public interface ITokenHashService
    {
        string Hash(string token);
        string GenerateRefreshToken();
    }
}
