using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Domain.ValueObjects
{
    public sealed record Status
    {
        public static readonly Status Todo = new(nameof(Todo));
        public static readonly Status InProgress = new(nameof(InProgress));
        public static readonly Status Done = new(nameof(Done));
        public static readonly Status Cancelled = new(nameof(Cancelled));

        private static readonly IReadOnlyCollection<Status> _all =
        [
            Todo, InProgress, Done, Cancelled
        ];

        public string Value { get; } = string.Empty;

        private Status() { }
        private Status(string value)
        {
            Value = value;
        }

        public static Result<Status> FromValue(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                return Result.Failure<Status>(DomainErrors.Status.Empty);

            var status = _all.FirstOrDefault(s =>
                string.Equals(
                    s.Value,
                    value.Trim(),
                    StringComparison.OrdinalIgnoreCase));

            return status is not null
                ? status
                : Result.Failure<Status>(DomainErrors.Status.Invalid);
        }

        public override string ToString() => Value;
    }
}
