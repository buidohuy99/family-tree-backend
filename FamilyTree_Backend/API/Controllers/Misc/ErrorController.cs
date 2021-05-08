using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;

namespace FamilyTreeBackend.Presentation.API.Controllers.Misc
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {

        public class MyErrorResponse
        {
            public string Type { get; set; }
            public string Message { get; set; }
            public string StackTrace { get; set; }

            public MyErrorResponse(Exception ex)
            {
                if (ex == null)
                {
                    Type = "Invalid Error";
                    Message = "Cannot find the exception because its null";
                    StackTrace = null;
                    return;
                }
                Type = ex.GetType().Name;
                Message = ex.Message;
                StackTrace = ex.StackTrace?.ToString();
            }
        }

        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Handler()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error; // Your exception

            //log exception
            Log.Logger.Error(exception, $"Unexpected System Exception: {exception.Message}");

            var code = 500; // Internal Server Error by default

            return StatusCode(code, new MyErrorResponse(exception)); // Your error model

        }
    }
}
