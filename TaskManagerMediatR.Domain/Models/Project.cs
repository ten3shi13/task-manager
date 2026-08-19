using TaskManagerMediatR.Domain.DomainEvents;
using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Primitives;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Domain.Models
{
    public class Project : AggregateRoot
    {
        public const int NAME_MAX_LENGTH = 100;
        public const int DESCRIPTION_MAX_LENGTH = 500;

        private readonly List<ProjectMember> _members = [];


        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; } = string.Empty;
        public Guid OwnerId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public IReadOnlyCollection<ProjectMember> Members => _members.AsReadOnly();

        private Project() {}

        private Project(Guid id, string name, string description, Guid ownerId) : base(id)
        {
            Name = name;
            Description = description;
            OwnerId = ownerId;
            CreatedAt = DateTime.UtcNow;

            _members.Add(new ProjectMember(ownerId, ProjectRole.Owner));
        }
        
        private static Result Validate(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(DomainErrors.Project.EmptyName);

            if (name.Length > NAME_MAX_LENGTH)
                return Result.Failure(DomainErrors.Project.InvalidNameLength);

            if (description.Length > DESCRIPTION_MAX_LENGTH)
                return Result.Failure(DomainErrors.Project.InvalidDescriptionLength);

            return Result.Success();
        }

        public static Result<Project> Create(Guid id, string name, string description, Guid ownerId)
        {
            var validationResult = Validate(name, description);

            if (validationResult.IsFailure)
                return Result.Failure<Project>(validationResult.Errors);

            return new Project(id, name.Trim(), description.Trim(), ownerId);
        }

        public Result UpdateDetails(string name, string description)
        {
            var validationResult = Validate(name, description);

            if (validationResult.IsFailure)
                return Result.Failure(validationResult.Errors);

            Name = name.Trim();
            Description = description.Trim();

            return Result.Success();
        }

        public Result AddMember(Guid userId)
        {
            if (_members.Any(m => m.UserId == userId))
                return Result.Failure(DomainErrors.Project.MemberAlreadyExists);

            _members.Add(new ProjectMember(userId, ProjectRole.Member));

            RaiseDomainEvent(new ProjectMemberAddedDomainEvent(Guid.NewGuid(), Id, userId));

            return Result.Success();
        }

        public Result RemoveMember(Guid userId)
        {
            if (userId == OwnerId)
                return Result.Failure(DomainErrors.Project.CannotRemoveOwner);

            var member = _members.FirstOrDefault(m => m.UserId == userId);
            if (member is null)
                return Result.Failure(DomainErrors.Project.MemberNotFound);

            _members.Remove(member);
            return Result.Success();
        }

        public bool IsMember(Guid userId) =>
            _members.Any(m => m.UserId == userId);

        public bool IsOwner(Guid userId) =>
            OwnerId == userId;
    }
}
