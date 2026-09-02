namespace TaskManagerMediatR.Infrastructure.Shared.Persistence.Authentication
{
    public sealed class JwtTokenOptions
    {
        public required string Issuer { get; init; }
        public required string Audience { get; init; }
        public required string SecretKey { get; init; }
        public int AccessTokenExpirationMinutes { get; init; } = 15;
        public int RefreshTokenExpirationDays { get; init; } = 7;
    }
}
