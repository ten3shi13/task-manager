using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Domain.ValueObjects
{
    public sealed record FirstName
    {
        public const int FIRST_NAME_MAX_LENGTH = 50;
        public string Value { get; } = string.Empty;

        private FirstName() { }
        private FirstName(string value) => Value = value;

        public static Result<FirstName> Create(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                return Result.Failure<FirstName>(DomainErrors.FirstName.Empty);
            }

            if (firstName.Length > FIRST_NAME_MAX_LENGTH)
            {
                return Result.Failure<FirstName>(DomainErrors.FirstName.InvalidLength);
            }

            return new FirstName(firstName);
        }

    }
}
