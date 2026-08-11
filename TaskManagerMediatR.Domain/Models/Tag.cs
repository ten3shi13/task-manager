using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Primitives;
using TaskManagerMediatR.Domain.Shared;
using TaskManagerMediatR.Domain.ValueObjects;

namespace TaskManagerMediatR.Domain.Models
{
    public class Tag : Entity
    {
        public const int NAME_MAX_LENGTH = 30;

        private Tag(){}
        private Tag(Guid id, string name, Color color) : base(id){
        
            Name = name;
            Color = color;
        }

        public string Name { get; private set; } = string.Empty;
        public Color Color { get; private set; } = null!;

        public static Result<Tag> Create(string name, Color color)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Tag>(DomainErrors.Tag.EmptyName);

            if (name.Length > NAME_MAX_LENGTH)
                return Result.Failure<Tag>(DomainErrors.Tag.InvalidNameLength);

            return new Tag(Guid.NewGuid(), name.Trim(), color);
        }

    }
}
