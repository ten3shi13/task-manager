using TaskManagerMediatR.Domain.Primitives;

namespace TaskManagerMediatR.Domain.Models
{
    public sealed class ProjectMember : Entity
    {
        private ProjectMember()
        {
        }

        internal ProjectMember(Guid userId, ProjectRole projectRole) 
        {
            UserId = userId;    
            ProjectRole = projectRole;
            JoinedAt = DateTime.UtcNow;
        }

        public Guid UserId { get; private set; }
        public ProjectRole ProjectRole { get; private set; }
        public DateTime JoinedAt { get; private set; }
    }

    public enum ProjectRole
    {
        Owner = 0,
        Member = 1,
    }
}
