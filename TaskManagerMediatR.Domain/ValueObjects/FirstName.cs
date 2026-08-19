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

        public static Result<FirstName> Create(string firstName) =>

            Result.Ensure(firstName,
                (fn => !string.IsNullOrWhiteSpace(fn), DomainErrors.FirstName.Empty),
                (fn => fn.Length <= FIRST_NAME_MAX_LENGTH, DomainErrors.FirstName.InvalidLength))
                .Map(fn => new FirstName(fn));

    }
}
