using LLMForum.Server.Exceptions.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LLMForum.Server.Exceptions.ExceptionHandling
{
    public class GlobalExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var statusCode = context.Exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                CustomUnauthorizedAccessException => StatusCodes.Status401Unauthorized,

                ExternalServiceException => StatusCodes.Status502BadGateway,
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
