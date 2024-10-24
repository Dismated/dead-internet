using DeadInternet.Server.Exceptions;
using DeadInternet.Server.Exceptions.Base;
using DeadInternet.Server.Exceptions.ExceptionHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace DeadInternet.Tests.Exceptions
{
    public class GlobalExceptionHandlerTests
    {
        private readonly GlobalExceptionHandler _handler = new GlobalExceptionHandler();

        [Fact]
        public void OnException_BadRequestException_Returns400()
        {
            // Arrange
            var exception = new BadRequestException("Bad request");
            var context = CreateExceptionContext(exception);

            // Act
            _handler.OnException(context);

            // Assert
            var result = Assert.IsType<ObjectResult>(context.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.True(context.ExceptionHandled);
        }

        [Fact]
        public void OnException_NotFoundException_Returns404()
        {
            // Arrange
            var exception = new NotFoundException("Not found");
            var context = CreateExceptionContext(exception);

            // Act
            _handler.OnException(context);

            // Assert
            var result = Assert.IsType<ObjectResult>(context.Result);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.True(context.ExceptionHandled);
        }

        [Fact]
        public void OnException_CustomUnauthorizedAccessException_Returns401()
        {
            // Arrange
            var exception = new CustomUnauthorizedAccessException("Unauthorized");
            var context = CreateExceptionContext(exception);

            // Act
            _handler.OnException(context);

            // Assert
            var result = Assert.IsType<ObjectResult>(context.Result);
            Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
            Assert.True(context.ExceptionHandled);
        }

        [Fact]
        public void OnException_ExernalServiceException_Returns502()
        {
            // Arrange
            var exception = new ExternalServiceException("External service error");
            var context = CreateExceptionContext(exception);

            // Act
            _handler.OnException(context);

            // Assert
            var result = Assert.IsType<ObjectResult>(context.Result);
            Assert.Equal(StatusCodes.Status502BadGateway, result.StatusCode);
            Assert.True(context.ExceptionHandled);
        }

        [Fact]
        public void OnException_UnhandledException_Returns500()
        {
            // Arrange
            var exception = new Exception("Unhandled exception");
            var context = CreateExceptionContext(exception);

            // Act
            _handler.OnException(context);

            // Assert
            var result = Assert.IsType<ObjectResult>(context.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
            Assert.True(context.ExceptionHandled);
        }

        private ExceptionContext CreateExceptionContext(Exception exception)
        {
            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new ActionDescriptor()
            );

            return new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = exception,
            };
        }
    }
}
