using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerMediatR.API.Abstractions;
using TaskManagerMediatR.Application.Shared.Authentication;
using TaskManagerMediatR.Application.Users.Queries.Get;
using TaskManagerMediatR.Application.Users.Queries.GetById;
using TaskManagerMediatR.Contracts.Users;
using TaskManagerMediatR.Infrastructure.Shared.Persistence.Authentication;

namespace TaskManagerMediatR.API.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    public sealed class UsersController : BaseApiController
    {
        public UsersController(ISender sender) : base(sender)
        {
        }

        [HasPermission(Permissions.UsersRead)]
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var userResult = await _sender.Send(new GetUsersQuery(), cancellationToken);

            return FromResult(userResult);
        }

        [HasPermission(Permissions.UsersRead)]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
        {
            var userResult = await _sender.Send(new GetUserQuery(id), cancellationToken);

            return FromResult(userResult);
        }

        //[HasPermission(Permissions.UsersManage)]
        //[HttpPut("{id:guid}/role")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> ChangeUserRole(Guid id, [FromBody] ChangeRoleRequest request, CancellationToken cancellationToken)
        //{
        //    var result = await Sender.Send(new ChangeUserRoleCommand(id, request.Role), cancellationToken);

        //    return FromResult(result);
        //}
    }
}
