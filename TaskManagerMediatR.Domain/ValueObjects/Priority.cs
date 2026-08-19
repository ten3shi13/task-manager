using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Domain.ValueObjects
{
    public sealed record Priority
    {
        public static readonly Priority Low = new(nameof(Low));
        public static readonly Priority Medium = new(nameof(Medium));
        public static readonly Priority High = new(nameof(High));
        public static readonly Priority Critical = new(nameof(Critical));

        private static readonly IReadOnlyCollection<Priority> _all =
        [
            Low, Medium, High, Critical
        ];

        public string Value { get; } = string.Empty;

        private Priority() { }
        private Priority(string value)
        { 
            Value = value;
        }

        public static Result<Priority> FromValue(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                return Result.Failure<Priority>(DomainErrors.Priority.Empty);

            var priority = _all.FirstOrDefault(p =>
                string.Equals(
                    p.Value,
                    value.Trim(),
                    StringComparison.OrdinalIgnoreCase));

            return priority is not null
                ? priority
                : Result.Failure<Priority>(DomainErrors.Priority.Invalid);
        }

        public override string ToString() => Value;
    }
}
