using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerMediatR.API.Abstractions;
using TaskManagerMediatR.Application.Shared.Authentication;
using TaskManagerMediatR.Application.Users.Queries.Get;
using TaskManagerMediatR.Application.Users.Queries.GetById;
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
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var userResult = await _sender.Send(new GetUsersQuery(), cancellationToken);

            return FromResult(userResult);
        }

        [HasPermission(Permissions.UsersRead)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
        {
            var userResult = await _sender.Send(new GetUserQuery(id), cancellationToken);

            return FromResult(userResult);
        }

    }
}
