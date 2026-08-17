using TaskManagerMediatR.Domain.DomainEvents;
using TaskManagerMediatR.Domain.Primitives;
using TaskManagerMediatR.Domain.Shared;
using TaskManagerMediatR.Domain.ValueObjects;
using DomainErrors = TaskManagerMediatR.Domain.Errors.DomainErrors;

namespace TaskManagerMediatR.Domain.Models
{
    public class Task : AggregateRoot
    {
        public const int TITLE_MAX_LENGTH = 100;
        public const int DESCRIPTION_MAX_LENGTH = 1000;

        private readonly List<Comment> _comments = [];
        private readonly List<Tag> _tags = [];
        private readonly List<Assignment> _assignments = [];

        private Task() { }
        private Task(
            Guid id,
            Guid projectId,
            string title,
            string description,
            Status status,
            Priority priority,
            Guid createdById,
            DateTime? dueDate) : base(id)
        {
            ProjectId = projectId;
            Title = title;
            Description = description;
            Status = status;
            Priority = priority;
            CreatedById = createdById;
            DueDate = dueDate;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }

        public Guid ProjectId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public Status Status { get; private set; } = null!;
        public Priority Priority { get; private set; } = null!;
        public DateTime? DueDate { get; private set; }
        public Guid CreatedById { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
        public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();
        public IReadOnlyCollection<Assignment> Assignments => _assignments.AsReadOnly();


        public static Result<Task> Create(
            Guid id,
            Guid projectId,
            string title,
            string description,
            Status status,
            Priority priority,
            Guid createdById,
            DateTime? dueDate = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<Task>(DomainErrors.Task.EmptyTitle);

            if (title.Length > TITLE_MAX_LENGTH)
                return Result.Failure<Task>(DomainErrors.Task.InvalidTitleLength);

            if (description.Length > DESCRIPTION_MAX_LENGTH)
                return Result.Failure<Task>(DomainErrors.Task.InvalidDescriptionLength);

            if (dueDate.HasValue && dueDate.Value.Date < DateTime.UtcNow.Date)
                return Result.Failure<Task>(DomainErrors.Task.DueDateInPast);

            return new Task(id, projectId, title.Trim(), description.Trim(), status, priority, createdById, dueDate);
        }

        public Result UpdateDetails(
            string title,
            string description,
            Priority priority,
            DateTime? dueDate)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure(DomainErrors.Task.EmptyTitle);

            if (title.Length > TITLE_MAX_LENGTH)
                return Result.Failure(DomainErrors.Task.InvalidTitleLength);

            if (description.Length > DESCRIPTION_MAX_LENGTH)
                return Result.Failure(DomainErrors.Task.InvalidDescriptionLength);

            Title = title.Trim();
            Description = description.Trim();
            Priority = priority;
            DueDate = dueDate;
            UpdatedAt = DateTime.UtcNow;

            return Result.Success();
        }

        public Result ChangeStatus(Status status)
        {
            if (Status == status)
                return Result.Success();

            if (Status == Status.Done)
                return Result.Failure(DomainErrors.Task.AlreadyCompleted);

            var previousStatus = Status;

            Status = status;
            UpdatedAt = DateTime.UtcNow;

            RaiseDomainEvent(new TaskStatusChangedDomainEvent(Guid.NewGuid(), Id));

            if (status == Status.Done)
            {
                RaiseDomainEvent(new TaskCompletedDomainEvent(Guid.NewGuid(), Id));
            }

            return Result.Success();
        }

        public Result Complete() => ChangeStatus(Status.Done);

        public Result AssignUser(Guid userId, Guid assignedBy)
        {
            if (_assignments.Any(a => a.UserId == userId))
                return Result.Failure(DomainErrors.Task.UserAlreadyAssigned);

            _assignments.Add(Assignment.Create(userId, assignedBy));
            UpdatedAt = DateTime.UtcNow;

            RaiseDomainEvent(new TaskAssignedDomainEvent(Guid.NewGuid(), Id, userId));

            return Result.Success();
        }

        public Result UnassignUser(Guid userId)
        {
            var assignment = _assignments.FirstOrDefault(a => a.UserId == userId);
            if (assignment is null)
                return Result.Failure(DomainErrors.Task.UserNotAssigned);

            _assignments.Remove(assignment);
            UpdatedAt = DateTime.UtcNow;

            RaiseDomainEvent(new TaskUnassignedDomainEvent(Guid.NewGuid(), Id, userId));

            return Result.Success();
        }

        public Result AddTag(string name, Color color)
        {
            if (_tags.Any(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                return Result.Failure(DomainErrors.Task.TagAlreadyExists);

            var tagResult = Tag.Create(name, color);
            if (tagResult.IsFailure)
                return Result.Failure(tagResult.Error);

            _tags.Add(tagResult.Value);
            UpdatedAt = DateTime.UtcNow;

            return Result.Success();
        }

        public Result RemoveTag(Guid tagId)
        {
            var tag = _tags.FirstOrDefault(t => t.Id == tagId);
            if (tag is null)
                return Result.Failure(DomainErrors.Task.TagNotFound);

            _tags.Remove(tag);
            UpdatedAt = DateTime.UtcNow;

            return Result.Success();
        }

        public Result<Comment> AddComment(Guid authorId, string text)
        {
            var commentResult = Comment.Create(authorId, text);
            if (commentResult.IsFailure)
                return Result.Failure<Comment>(commentResult.Error);

            _comments.Add(commentResult.Value);
            UpdatedAt = DateTime.UtcNow;

            return commentResult.Value;
        }

        public Result EditComment(Guid commentId, Guid editorId, string newText)
        {
            var comment = _comments.FirstOrDefault(c => c.Id == commentId);
            if (comment is null)
                return Result.Failure(DomainErrors.Task.CommentNotFound);

            return comment.Edit(editorId, newText);
        }

        public Result DeleteComment(Guid commentId, Guid requesterId)
        {
            var comment = _comments.FirstOrDefault(c => c.Id == commentId);
            if (comment is null)
                return Result.Failure(DomainErrors.Task.CommentNotFound);

            if (comment.AuthorId != requesterId)
                return Result.Failure(DomainErrors.Task.OnlyAuthorCanDeleteComment);

            _comments.Remove(comment);
            UpdatedAt = DateTime.UtcNow;

            return Result.Success();
        }
    }
}
