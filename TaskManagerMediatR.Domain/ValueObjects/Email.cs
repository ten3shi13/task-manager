using System.Text.RegularExpressions;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Domain.ValueObjects
{
    public sealed record Email
    {
        public const string EMAIL_PATTERN = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        public const int EMAIL_MAX_LENGTH = 254;
        public string Value { get; } = string.Empty;

        private Email() { }
        private Email(string value) => Value = value;

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Result.Failure<Email>(DomainErrors.Email.Empty);
            }

            if (!Regex.IsMatch(email, EMAIL_PATTERN))
            {
                return Result.Failure<Email>(DomainErrors.Email.InvalidFormat);
            }

            if (email.Length > EMAIL_MAX_LENGTH)
            {
                return Result.Failure<Email>(DomainErrors.Email.InvalidLength);
            }

            return new Email(email.Trim());
        }

    }
}
