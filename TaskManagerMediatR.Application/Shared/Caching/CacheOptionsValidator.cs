using Microsoft.Extensions.Options;

namespace TaskManagerMediatR.Application.Shared.Caching
{
    public sealed class CacheOptionsValidator : IValidateOptions<CacheOptions>
    {
        public ValidateOptionsResult Validate(string? name, CacheOptions options)
        {
            var errors = new List<string>();

            if (options.DefaultExpiration <= TimeSpan.Zero)
                errors.Add("DefaultExpiration must be greater than zero.");

            if (options.ProjectExpiration <= TimeSpan.Zero)
                errors.Add("ProjectExpiration must be greater than zero.");

            if (options.TaskExpiration <= TimeSpan.Zero)
                errors.Add("TaskExpiration must be greater than zero.");

            if (options.Tasks.DefaultExpiration <= TimeSpan.Zero)
                errors.Add("Tasks.DefaultExpiration must be greater than zero.");

            if (options.Tasks.FilteredExpiration <= TimeSpan.Zero)
                errors.Add("Tasks.FilteredExpiration must be greater than zero.");

            if (options.Tasks.SearchExpiration <= TimeSpan.Zero)
                errors.Add("Tasks.SearchExpiration must be greater than zero.");

            if (options.Projects.DefaultExpiration <= TimeSpan.Zero)
                errors.Add("Projects.DefaultExpiration must be greater than zero.");

            if (options.Projects.FilteredExpiration <= TimeSpan.Zero)
                errors.Add("Projects.FilteredExpiration must be greater than zero.");

            if (options.Projects.SearchExpiration <= TimeSpan.Zero)
                errors.Add("Projects.SearchExpiration must be greater than zero.");

            return errors.Count > 0 
                ? ValidateOptionsResult.Fail(errors) 
                : ValidateOptionsResult.Success;
        }
    }
}
