using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagerMediatR.API.Mappers;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.API.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly ISender _sender;
        protected BaseApiController(ISender sender)
        {
            _sender = sender;
        }

        protected IActionResult FromResult(Result result)
        {
            return result.IsSuccess
                ? NoContent()
                : Problem(result.Error);
        }

        protected IActionResult FromResult<T>(Result<T> result)
        {
            return result.IsSuccess
                ? Ok(result.Value)
                : Problem(result.Error);
        }

        protected IActionResult CreatedAtActionResult<T>(Result<T> result, string actionName, object? routeValues = null)
        {
            return result.IsFailure 
                ? Problem(result.Error) 
                : CreatedAtAction(actionName, routeValues, result.Value);
        }

        protected IActionResult Problem(Error error)
        {
            var statusCode = ErrorMapper.ToStatusCode(error);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = error.Code,
                Detail = error.Message,
                Instance = HttpContext.Request.Path,
                Type = $"https://httpstatuses.com/{statusCode}"
            };

            problemDetails.Extensions["errorCode"] = error.Code;
            problemDetails.Extensions["traceId"] = HttpContext.TraceIdentifier;

            return new ObjectResult(problemDetails)
            {
                StatusCode = statusCode
            };
        }

    }
}
