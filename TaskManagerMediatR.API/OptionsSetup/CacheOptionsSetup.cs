using Microsoft.Extensions.Options;
using TaskManagerMediatR.Application.Shared.Caching;

namespace TaskManagerMediatR.API.OptionsSetup
{
    public class CacheOptionsSetup(IConfiguration configuration) : IConfigureOptions<CacheOptions>
    {
        private const string SectionName = "Cache";

        public void Configure(CacheOptions options) =>
            configuration.GetSection(SectionName).Bind(options);
    }
}
