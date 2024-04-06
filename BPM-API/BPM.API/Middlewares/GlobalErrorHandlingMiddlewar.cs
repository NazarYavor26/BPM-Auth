using BPM.BLL.Models;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;

namespace BPM.API.Middlewares
{
    public sealed class GlobalErrorHandlingMiddlewar
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddlewar(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(context, exception);
            }
        }

        private async Task HandleErrorAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;
            ErrorDetails errorDetails = new ErrorDetails();

            switch (exception)
            {
                case InvalidCredentialException ex:
                    errorDetails.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorDetails.Message = exception.Message;
                    break;

                default:
                    errorDetails.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorDetails.Message = "Internal server error. Please retry after some time.";
                    break;
            }

            var exceptionResult = JsonSerializer.Serialize(errorDetails);
            await context.Response.WriteAsync(exceptionResult);
        }
    }
}
