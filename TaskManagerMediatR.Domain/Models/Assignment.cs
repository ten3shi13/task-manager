using TaskManagerMediatR.Domain.Primitives;

namespace TaskManagerMediatR.Domain.Models
{
    public sealed class Assignment : Entity
    {
        private Assignment() { }

        private Assignment(Guid id, Guid userId, Guid assignedBy) : base(id)
        {
            UserId = userId;
            AssignedBy = assignedBy;
            AssignedAt = DateTime.UtcNow;
        }

        public Guid UserId { get; private set; }
        public Guid AssignedBy { get; private set; }
        public DateTime AssignedAt { get; private set; }

        public static Assignment Create(Guid userId, Guid assignedBy)
        {
            return new Assignment(Guid.NewGuid(), userId, assignedBy);
        }
    }
}
