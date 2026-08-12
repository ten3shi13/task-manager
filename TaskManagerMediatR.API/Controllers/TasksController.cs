using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagerMediatR.Application.Shared.Abstractions.Authentication;
using TaskManagerMediatR.Application.Tasks.Commands.AddCommentToTask;
using TaskManagerMediatR.Application.Tasks.Commands.AddTagToTask;
using TaskManagerMediatR.Application.Tasks.Commands.AssignUserToTask;
using TaskManagerMediatR.Application.Tasks.Commands.ChangeTaskStatus;
using TaskManagerMediatR.Application.Tasks.Commands.Create;
using TaskManagerMediatR.Application.Tasks.Commands.DeleteComment;
using TaskManagerMediatR.Application.Tasks.Commands.EditComment;
using TaskManagerMediatR.Application.Tasks.Commands.RemoveTagFromTask;
using TaskManagerMediatR.Application.Tasks.Commands.UnassignUserFromTask;
using TaskManagerMediatR.Application.Tasks.Commands.Update;
using TaskManagerMediatR.Application.Tasks.Queries.GetTask;
using TaskManagerMediatR.Application.Tasks.Queries.GetTasksByProject;
using TaskManagerMediatR.Contracts.Tasks;

namespace TaskManagerMediatR.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public sealed class TasksController : BaseApiController
    {
        private readonly ICurrentUser _currentUser;
        public TasksController(ISender sender, ICurrentUser currentUser) : base(sender)
        {
            _currentUser = currentUser;
        }

        [HttpGet("{projectId:guid}/tasks")]
        [ProducesResponseType(typeof(IReadOnlyList<TaskResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTasksByProject(Guid projectId, CancellationToken cancellationToken)
        {
            var tasksResult = await _sender.Send(new GetTasksByProjectQuery(projectId), cancellationToken);

            return FromResult(tasksResult);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTask(Guid id, CancellationToken cancellationToken)
        {
            var taskResult = await _sender.Send(new GetTaskQuery(id), cancellationToken);

            return FromResult(taskResult);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTask(CreateTaskRequest request, CancellationToken cancellationToken)
        {
            var taskResult = await _sender.Send(new CreateTaskCommand(
                request.ProjectId,
                request.Title,
                request.Description,
                request.Priority,
                _currentUser.UserId,
                request.DueDate), cancellationToken);

            if (taskResult.IsFailure)
                return Problem(taskResult.Error);

            return CreatedAtAction(
                nameof(GetTask),
                new { id = taskResult.Value },
                taskResult.Value);

        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTask(Guid id, UpdateTaskRequest request, CancellationToken cancellationToken)
        {
            var taskResult = await _sender.Send(new UpdateTaskCommand(
                id,
                request.Title,
                request.Description,
                request.Priority,
                request.DueDate,
                _currentUser.UserId), cancellationToken);

            return FromResult(taskResult);

        }

        //[HttpDelete("{id:guid}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> DeleteTask(Guid id, CancellationToken cancellationToken)
        //{
        //}

        [HttpPost("{taskId:guid}/assignments/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignUser(Guid taskId, Guid userId, CancellationToken cancellationToken)
        {
            var assignUserResult = await _sender.Send(new AssignUserToTaskCommand(
                taskId, userId, _currentUser.UserId), cancellationToken);

            return FromResult(assignUserResult);

        }

        [HttpDelete("{taskId:guid}/assignments/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnassignUser(Guid taskId, Guid userId, CancellationToken cancellationToken)
        {
            var unassignUserResult = await _sender.Send(new UnassignUserFromTaskCommand(
                taskId, userId, _currentUser.UserId), cancellationToken);

            return FromResult(unassignUserResult);

        }

        [HttpPost("{id:guid}/tags")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddTag(Guid id, AddTagToTaskRequest request, CancellationToken cancellationToken)
        {
            var tagResult = await _sender.Send(new AddTagToTaskCommand(
                id, request.Name, request.Code, _currentUser.UserId), cancellationToken);

            return FromResult(tagResult);

        }

        [HttpDelete("{id:guid}/tags/{tagId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveTag(Guid id, Guid tagId, CancellationToken cancellationToken)
        {
            var tagResult = await _sender.Send(new RemoveTagFromTaskCommand(
                id, tagId, _currentUser.UserId), cancellationToken);

            return FromResult(tagResult);

        }

        [HttpPost("{id:guid}/comments")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddComment(Guid id, AddCommentToTaskRequest request, CancellationToken cancellationToken)
        {
            var commentResult = await _sender.Send(new AddCommentToTaskCommand(
                id, request.Text, _currentUser.UserId), cancellationToken);

            return FromResult(commentResult);

        }

        [HttpPut("{id:guid}/comments/{commentId:guid}")]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditComment(Guid id, Guid commentId, EditCommentRequest request, CancellationToken cancellationToken)
        {
            var commentResult = await _sender.Send(new EditCommentCommand(
                id, commentId, request.Text, _currentUser.UserId), cancellationToken);

            return FromResult(commentResult);

        }

        [HttpDelete("{id:guid}/comments/{commentId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveComment(Guid id, Guid commentId, CancellationToken cancellationToken)
        {
            var commentResult = await _sender.Send(new DeleteCommentCommand(
                id, commentId, _currentUser.UserId), cancellationToken);

            return FromResult(commentResult);

        }

        [HttpPatch("{id:guid}/status")]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeStatus(Guid id, ChangeTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var commentResult = await _sender.Send(new ChangeTaskStatusCommand(
                id, request.Status, request.ChangedById), cancellationToken);

            return FromResult(commentResult);

        }

    }
}
