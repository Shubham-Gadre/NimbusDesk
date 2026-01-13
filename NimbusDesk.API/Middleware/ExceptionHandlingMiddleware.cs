using NimbusDesk.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace NimbusDesk.API.Middleware
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

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
