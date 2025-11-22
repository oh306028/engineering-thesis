
using Thesis.app.Exceptions;
namespace Thesis.api
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException ex)
            {
                await WriteJsonError(context, 404, ex.Message);
            }
            catch (WrongAnswerException ex)
            {
                await WriteJsonError(context, 422, ex.Message);
            }
            catch (Exception ex)
            {
                await WriteJsonError(context, 500, "Wystąpił nieoczekiwany błąd.");
            }
        }

        private async Task WriteJsonError(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var error = new
            {
                status = statusCode,
                error = message
            };

            var json = System.Text.Json.JsonSerializer.Serialize(error);

            await context.Response.WriteAsync(json);
        }
    }

}
