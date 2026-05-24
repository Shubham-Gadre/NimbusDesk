using NimbusDesk.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace NimbusDesk.API.Middleware
{
    /// <summary>
    /// Middleware for global exception handling.
    /// Catches domain exceptions and unhandled exceptions, returning appropriate HTTP responses with error messages.
    /// </summary>
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware delegate in the pipeline.</param>
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the middleware to process the HTTP request and handle any exceptions.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                await HandleDomainExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleUnhandledExceptionAsync(context, ex);
            }

        }

        /// <summary>
        /// Handles domain exceptions by returning a 400 Bad Request response.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="exception">The domain exception to handle.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private static async Task HandleDomainExceptionAsync(
            HttpContext context,
            DomainException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = exception.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        /// <summary>
        /// Handles unhandled exceptions by returning a 500 Internal Server Error response.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="exception">The unhandled exception to handle.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private static async Task HandleUnhandledExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = "An unexpected error occurred."
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
