using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LLMForum.Server.Exceptions
{
    public class GlobalExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var statusCode = context.Exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                // Add other exception types here
                _ => StatusCodes.Status500InternalServerError,
            };

            context.Result = new ObjectResult(new { error = context.Exception.Message })
            {
                StatusCode = statusCode,
            };
            context.ExceptionHandled = true;
        }
    }
}
