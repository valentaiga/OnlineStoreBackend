using System.Net;
using Microsoft.AspNetCore.Mvc;
using OnlineStoreBackend.Abstractions.Models;
using OnlineStoreBackend.Api.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace OnlineStoreBackend.Api.Controllers;

[ApiController]
[SwaggerResponse((int)HttpStatusCode.OK)]
[SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad request response", typeof(BadRequestResponse))]
[SwaggerResponse((int)HttpStatusCode.InternalServerError, "Unhandled exception", typeof(InternalExceptionResponse))]
public class ApiControllerBase : ControllerBase
{
    [NonAction]
    protected ActionResult Process<T, TResponse>(Result<T> result, Func<Result<T>, TResponse> processSuccess)
    {
        return result.IsSuccess
            ? Ok(processSuccess(result))
            : BadRequest(new BadRequestResponse(result.Error));
    }

    [NonAction]
    protected ActionResult Process(Result result)
    {
        return result.IsSuccess
            ? Ok()
            : BadRequest(new BadRequestResponse(result.Error));
    }
}