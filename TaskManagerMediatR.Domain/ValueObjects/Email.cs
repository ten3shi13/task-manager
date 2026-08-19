using System.Text.RegularExpressions;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Domain.ValueObjects
{
    public sealed partial record Email
    {
        public const string EMAIL_PATTERN = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        [GeneratedRegex(EMAIL_PATTERN)]
        private static partial Regex EmailRegex();

        public const int EMAIL_MAX_LENGTH = 254;
        public string Value { get; } = string.Empty;

        private Email() { }
        private Email(string value) => Value = value;

        public static Result<Email> Create(string email) =>
        
            Result.Ensure(email,
                (e => !string.IsNullOrWhiteSpace(e), DomainErrors.Email.Empty),
                (e => EmailRegex().IsMatch(e.Trim()), DomainErrors.Email.InvalidFormat),
                (e => e.Length <= EMAIL_MAX_LENGTH, DomainErrors.Email.InvalidLength))
                .Map(e => new Email(e.Trim()));
        
    }
}
