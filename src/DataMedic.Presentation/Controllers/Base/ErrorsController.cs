using System.Net;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DataMedic.Presentation.Controllers.Base;

/// <summary>
/// Errors controller
/// </summary>
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{
    /// <summary>
    /// Error endpoint for handling exceptions
    /// </summary>
    /// <returns></returns>
    [Route("error")]
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        return Problem(statusCode: (int)HttpStatusCode.InternalServerError, title: exception?.Message);
    }
}