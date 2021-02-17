using HARRepo.FileManager.API.Models;
using HARRepo.FileManager.Logic.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HARRepo.FileManager.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        [Route("error")]
        public ErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;
            var code = 500;

            if (exception is NotFoundException) code = 404; // Not Found
            else if (exception is AccessDeniedException) code = 401; // Unauthorized
            else if (exception is LogicException) code = 400; // Bad Request

            Response.StatusCode = code;

            return new ErrorResponse(exception);
        }
    }
}
