namespace TaskManagerMediatR.Domain.Shared
{
    public sealed record Error(string Code, string Message, ErrorType ErrorType)
    {
        public static readonly Error None =
            new(string.Empty, string.Empty, ErrorType.Validation);

        public static readonly Error NullValue =
            new("Error.NullValue", "The specified result value is null.", ErrorType.Validation);

        public static Error Failure(string code, string message) =>
            new(code, message, ErrorType.Failure);

        public static Error Validation(string code, string message) =>
            new(code, message, ErrorType.Validation);

        public static Error NotFound(string code, string message) =>
            new(code, message, ErrorType.NotFound);

        public static Error Conflict(string code, string message) =>
            new(code, message, ErrorType.Conflict);

        public static Error Unauthorized(string code, string message) =>
            new(code, message, ErrorType.Unauthorized);

        public static Error Forbidden(string code, string message) =>
            new(code, message, ErrorType.Forbidden);

        public static implicit operator Result(Error error) => Result.Failure(error);
    }
}
