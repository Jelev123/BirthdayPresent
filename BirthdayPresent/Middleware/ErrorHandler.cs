namespace BirthdayPresent.Middleware
{
    using BirthdayPresent.Core.Handlers;
    using BirthdayPresent.Models;
    using System.Net;
    using System.Text.Json;

    public class ErrorHandler
    {
        private readonly RequestDelegate next;

        public ErrorHandler(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = error switch
                {
                    ArgumentException => (int)HttpStatusCode.Conflict,
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                    NullReferenceException => (int)HttpStatusCode.BadRequest,
                    ResourceAlreadyExistsException => (int)HttpStatusCode.BadRequest,
                    ResourceNotFoundException => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError,
                };
                var result = JsonSerializer.Serialize(new ErrorViewModel { RequestId = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
