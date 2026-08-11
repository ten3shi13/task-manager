using TaskManagerMediatR.Domain.Errors;
using TaskManagerMediatR.Domain.Primitives;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Domain.Models
{
    public class Comment : Entity
    {
        public const int TEXT_MAX_LENGTH = 1000;

        private Comment(){}

        private Comment(Guid id, Guid authorId, string text) : base(id)
        {
            AuthorId = authorId;
            Text = text;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid AuthorId { get; private set; }
        public string Text { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public DateTime? EditedAt { get; private set; }

        public static Result<Comment> Create(Guid authorId, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return Result.Failure<Comment>(DomainErrors.Comment.EmptyText);

            if (text.Length > TEXT_MAX_LENGTH)
                return Result.Failure<Comment>(DomainErrors.Comment.InvalidTextLength);

            return new Comment(Guid.NewGuid(), authorId, text.Trim());
        }

        public Result Edit(Guid editorId, string newText)
        {
            if (AuthorId != editorId)
                return Result.Failure(DomainErrors.Comment.OnlyAuthorCanEdit);

            if (string.IsNullOrWhiteSpace(newText))
                return Result.Failure(DomainErrors.Comment.EmptyText);

            if (newText.Length > TEXT_MAX_LENGTH)
                return Result.Failure(DomainErrors.Comment.InvalidTextLength);

            Text = newText.Trim();
            EditedAt = DateTime.UtcNow;

            return Result.Success();
        }
    }
}
