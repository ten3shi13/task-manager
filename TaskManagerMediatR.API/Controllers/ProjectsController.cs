using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagerMediatR.API.Abstractions;
using TaskManagerMediatR.Contracts.Projects;
using TaskManagerMediatR.Application.Projects.Commands.Create;
using TaskManagerMediatR.Application.Projects.Commands.Delete;
using TaskManagerMediatR.Application.Projects.Commands.Update;
using TaskManagerMediatR.Application.Projects.Queries.GetById;
using TaskManagerMediatR.Application.Projects.Commands.AddProjectMember;
using TaskManagerMediatR.Application.Projects.Queries.GetProjectsByUser;
using TaskManagerMediatR.Application.Shared.Abstractions.Authentication;
using TaskManagerMediatR.Application.Projects.Commands.RemoveProjectMember;

namespace TaskManagerMediatR.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    //[Route("api/v{version:apiVersion}/[controller]")]   
    public sealed class ProjectsController : BaseApiController
    {
        private readonly ICurrentUser _currentUser;
        public ProjectsController(ISender sender, ICurrentUser currentUser) : base(sender)
        {
            _currentUser = currentUser;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ProjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjects([FromQuery] FilterProjectsRequest request, CancellationToken cancellationToken)
        {
            var projectsResult = await _sender.Send(new GetProjectsByUserQuery(
                _currentUser.UserId,
                request.Page,
                request.PageSize,
                request.Search,
                request.MemberId,
                request.SortBy,
                request.SortOrder),
            cancellationToken);

            return FromResult(projectsResult);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectById(Guid id,  CancellationToken cancellationToken)
        {
            var projectResult = await _sender.Send(new GetProjectQuery(id), cancellationToken);

            return FromResult(projectResult);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProject(CreateProjectRequest request, CancellationToken cancellationToken)
        {
            var projectResult = await _sender.Send(new CreateProjectCommand(request.Name, request.Description, _currentUser.UserId), cancellationToken);

            if (projectResult.IsFailure)
                    return Problem(projectResult.Errors);

            return CreatedAtAction(
                nameof(GetProjectById),
                new { id = projectResult.Value },
                projectResult.Value);

        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProject(Guid id, UpdateProjectRequest request, CancellationToken cancellationToken)
        {
            var projectResult = await _sender.Send(new UpdateProjectCommand(id, request.Name, request.Description, _currentUser.UserId), cancellationToken);

            return FromResult(projectResult);

        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProject(Guid id, CancellationToken cancellationToken)
        {
            var projectResult = await _sender.Send(new DeleteProjectCommand(id), cancellationToken);

            return FromResult(projectResult);

        }

        [HttpPost("{projectId:guid}/members/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddMember(Guid projectId, Guid userId, CancellationToken cancellationToken)
        {
            var addProjectMemberResult = await _sender.Send(new AddProjectMemberCommand(projectId, userId, _currentUser.UserId), cancellationToken);

            return FromResult(addProjectMemberResult);

        }

        [HttpDelete("{projectId:guid}/members/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveMember(Guid projectId, Guid userId, CancellationToken cancellationToken)
        {
            var removeProjectMemberResult = await _sender.Send(new RemoveProjectMemberCommand(projectId, userId, _currentUser.UserId), cancellationToken);

            return FromResult(removeProjectMemberResult);

        }
    }
}
