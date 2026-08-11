using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Domain.ValueObjects
{
    public sealed record Color
    {
        public static readonly Color Red = new("#E05C4D");
        public static readonly Color Orange = new("#D98B2B");
        public static readonly Color Green = new("#4CAF50");
        public static readonly Color Teal = new("#26A69A");
        public static readonly Color Blue = new("#5C6BC0");
        public static readonly Color Purple = new("#AB47BC");
        public static readonly Color Gray = new("#78909C");

        private static readonly IReadOnlyCollection<Color> _all =
        [
            Red, Orange, Green, Teal, Blue, Purple, Gray
        ];

        public string Code { get; } = string.Empty;

        private Color() { }
        private Color(string value)
        {
            Code = value;
        }

        public static Result<Color> FromCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return Result.Failure<Color>(DomainErrors.Color.Empty);
            }

            var match = _all.FirstOrDefault(p =>
                        string.Equals(p.Code, code.Trim(), StringComparison.OrdinalIgnoreCase));

            if (match is null)
            {
                return Result.Failure<Color>(DomainErrors.Priority.Invalid);
            }

            return match;
        }

        public override string ToString() => Code;
    }
}
