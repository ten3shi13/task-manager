using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagerMediatR.API.Mappers;
using TaskManagerMediatR.Domain.Shared;

namespace TaskManagerMediatR.API.Abstractions
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
                : Problem(result.Errors);
        }

        protected IActionResult FromResult<T>(Result<T> result)
        {
            return result.IsSuccess
                ? Ok(result.Value)
                : Problem(result.Errors);
        }

        protected IActionResult CreatedAtActionResult<T>(Result<T> result, string actionName, object? routeValues = null)
        {
            return result.IsFailure 
                ? Problem(result.Errors) 
                : CreatedAtAction(actionName, routeValues, result.Value);
        }

        protected IActionResult Problem(params Error[] errors)
        {
            if (errors is null || errors.Length == 0)
            {
                errors = [Error.Failure("Error.Unknown", "An unknown error occurred.")];
            }

            if (errors.Length == 1)
            {
                return ProblemSingle(errors[0]);
            }

            var statusCode = errors.Max(ErrorMapper.ToStatusCode);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = "Multiple errors occurred",
                Detail = "See the 'errors' field for details.",
                Instance = HttpContext.Request.Path,
                Type = $"https://httpstatuses.com/{statusCode}"
            };

            problemDetails.Extensions["errors"] = errors.Select(e => new
            {
                code = e.Code,
                message = e.Message,
                type = e.ErrorType.ToString()
            });

            problemDetails.Extensions["traceId"] = HttpContext.TraceIdentifier;

            return new ObjectResult(problemDetails)
            {
                StatusCode = statusCode
            };
        }

        private ObjectResult ProblemSingle(Error error)
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
            problemDetails.Extensions["errors"] = new[]
            {
            new
            {
                code = error.Code,
                message = error.Message,
                type = error.ErrorType.ToString()
            }
        };
            problemDetails.Extensions["traceId"] = HttpContext.TraceIdentifier;

            return new ObjectResult(problemDetails)
            {
                StatusCode = statusCode
            };
        }

    }
}
