using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.Domain.Errors
{
    public static class DomainErrors
    {
        public static class User
        {
            public static readonly Error EmptyPassword =
                Error.Validation("User.EmptyPassword", "Password hash cannot be empty");

            public static readonly Error NotFound =
                Error.NotFound("User.NotFound", "User was not found");

        }

        public static class Email
        {
            public static readonly Error Empty =
                Error.Validation("Email.Empty", "Email cannot be empty");

            public static readonly Error InvalidLength =
                Error.Validation("Email.InvalidLength", "Email is too long");

            public static readonly Error InvalidFormat =
                Error.Validation("Email.InvalidFormat", "Email format is invalid");
        }

        public static class FirstName
        {
            public static readonly Error Empty =
                Error.Validation("FirstName.Empty", "First name cannot be empty");

            public static readonly Error InvalidLength =
                Error.Validation("FirstName.InvalidLength", "First name is too long");
        }

        public static class Status
        {
            public static readonly Error Empty =
                Error.Validation("Status.Empty", "Status cannot be empty");

            public static readonly Error Invalid =
                Error.Validation("Status.Invalid", "Status is invalid");
        }

        public static class Priority
        {
            public static readonly Error Empty =
                Error.Validation("Priority.Empty", "Priority cannot be empty");

            public static readonly Error Invalid =
                Error.Validation("Priority.Invalid", "Priority is invalid");
        }

        public static class Color
        {
            public static readonly Error Empty =
                Error.Validation("Color.Empty", "Color cannot be empty");

            public static readonly Error Invalid =
                Error.Validation("Color.Invalid", "Color is invalid");
        }

        public static class Project
        {
            public static readonly Error EmptyName =
                Error.Validation("Project.EmptyName", "Project name cannot be empty");

            public static readonly Error InvalidNameLength =
                Error.Validation("Project.InvalidNameLength", "Project name is too long");

            public static readonly Error InvalidDescriptionLength =
                Error.Validation("Project.InvalidDescriptionLength", "Project description is too long");

            public static readonly Error NotFound =
                Error.NotFound("Project.NotFound", "Project was not found");

            public static readonly Error MemberAlreadyExists =
                Error.Conflict("Project.MemberAlreadyExists", "User is already a member of the project");

            public static readonly Error MemberNotFound =
                Error.NotFound("Project.MemberNotFound", "User is not a member of the project");

            public static readonly Error CannotRemoveOwner =
                Error.Conflict("Project.CannotRemoveOwner", "Cannot remove the project owner");

            public static readonly Error UserIsNotMember =
                Error.Forbidden("Project.UserIsNotMember", "User is not a member of the project");
        }

        public static class Task
        {
            public static readonly Error EmptyTitle =
                Error.Validation("Task.EmptyTitle", "Task title cannot be empty");

            public static readonly Error InvalidTitleLength =
                Error.Validation("Task.InvalidTitleLength", "Task title is too long");

            public static readonly Error InvalidDescriptionLength =
                Error.Validation("Task.InvalidDescriptionLength", "Task description is too long");

            public static readonly Error DueDateInPast =
                Error.Validation("Task.DueDateInPast", "Due date cannot be in the past");

            public static readonly Error NotFound =
                Error.NotFound("Task.NotFound", "Task was not found");

            public static readonly Error UserAlreadyAssigned =
                Error.Conflict("Task.UserAlreadyAssigned", "User is already assigned to the task");

            public static readonly Error UserNotAssigned =
                Error.NotFound("Task.UserNotAssigned", "User is not assigned to the task");

            public static readonly Error TagAlreadyExists =
                Error.Conflict("Task.TagAlreadyExists", "Tag already exists on the task");

            public static readonly Error TagNotFound =
                Error.NotFound("Task.TagNotFound", "Tag was not found on the task");

            public static readonly Error CommentNotFound =
                Error.NotFound("Task.CommentNotFound", "Comment was not found");

            public static readonly Error OnlyAuthorCanDeleteComment =
                Error.Forbidden("Task.OnlyAuthorCanDeleteComment", "Only the author can delete the comment");
        }

        public static class Comment
        {
            public static readonly Error EmptyText =
                Error.Validation("Comment.EmptyText", "Comment text cannot be empty");

            public static readonly Error InvalidTextLength =
                Error.Validation("Comment.InvalidTextLength", "Comment text is too long");

            public static readonly Error OnlyAuthorCanEdit =
                Error.Forbidden("Comment.OnlyAuthorCanEdit", "Only the author can edit the comment");
        }

        public static class Tag
        {
            public static readonly Error EmptyName =
                Error.Validation("Tag.EmptyName", "Tag name cannot be empty");

            public static readonly Error InvalidNameLength =
                Error.Validation("Tag.InvalidNameLength", "Tag name is too long");

            public static readonly Error InvalidColor =
                Error.Validation("Tag.InvalidColor", "Tag color is invalid");
        }
    }
}
