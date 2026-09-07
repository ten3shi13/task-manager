using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerMediatR.API.Abstractions;
using TaskManagerMediatR.Application.Users.Commands.Login;
using TaskManagerMediatR.Application.Users.Commands.Logout;
using TaskManagerMediatR.Application.Users.Commands.Refresh;
using TaskManagerMediatR.Application.Users.Commands.Register;
using TaskManagerMediatR.Contracts.Authentication;
using TaskManagerMediatR.Contracts.Authentication.Login;
using TaskManagerMediatR.Contracts.Authentication.Register;

namespace TaskManagerMediatR.API.Controllers
{
    [Route("api/[controller]")]
    public sealed class AuthController : BaseApiController
    {
        public AuthController(ISender sender) : base(sender)
        {
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _sender.Send(new LoginCommand(
                request.Email,
                request.Password),
                cancellationToken);

            return FromResult(authResult);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var registerResult = await _sender.Send(new RegisterCommand(
                request.FirstName,
                request.Email,
                request.Password),
                cancellationToken);

            if (registerResult.IsFailure)
                return Problem(registerResult.Errors);

            return Created($"/api/users/{registerResult.Value}", registerResult.Value);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new RefreshCommand(request.RefreshToken), cancellationToken);

            return FromResult(result);
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout([FromBody] RefreshRequest request, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new LogoutCommand(request.RefreshToken), cancellationToken);

            return FromResult(result);
        }
    }
}
