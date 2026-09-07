using Microsoft.Extensions.Options;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Authentication;

namespace TaskManagerMediatR.API.OptionsSetup
{
    public sealed class JwtTokenOptionsSetup(IConfiguration configuration) : IConfigureOptions<JwtTokenOptions>
    {
        private const string SectionName = "Jwt";

        public void Configure(JwtTokenOptions options) =>
            configuration.GetSection(SectionName).Bind(options);
    }
}
