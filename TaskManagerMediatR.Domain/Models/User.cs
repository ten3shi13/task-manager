using TaskManagerMediatR.Domain.Primitives;
using TaskManagerMediatR.Domain.Shared;
using TaskManagerMediatR.Domain.ValueObjects;

namespace TaskManagerMediatR.Domain.Models
{
    public class User : AggregateRoot
    {
        public const int NAME_MAX_LENGTH = 32;

        private User() { }

        private User(Guid id,
            FirstName firstName,
            Email email,
            string passwordHash) : base(id)
        {
            FirstName = firstName;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
        }


        public FirstName FirstName { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }


        public static Result<User> Create(Guid id,
            FirstName firstName,
            Email email,
            string passwordHash)
        {

            return new User(id, firstName, email, passwordHash);
        }

        public Result ChangeName(FirstName firstName)
        {
            FirstName = firstName;
            return Result.Success();
        }

    }
}
