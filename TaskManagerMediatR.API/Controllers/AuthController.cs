using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TaskManagerMediatR.API.Abstractions;
using TaskManagerMediatR.Application.Users.Commands.Login;

namespace TaskManagerMediatR.API.Controllers
{
    [Route("api/[controller]")]
    public sealed class AuthController : BaseApiController
    {
        public AuthController(ISender sender) : base(sender)
        {
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _sender.Send(new LoginCommand(
                request.Email,
                request.Password),
                cancellationToken);

            return FromResult(authResult);
        }
    }
}
