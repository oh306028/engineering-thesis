
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
            catch(NotFoundException notFoundEx)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFoundEx.Message);
            }
            catch (WrongAnswerException wrongAnswEx)
            {
                context.Response.StatusCode = 422;
                await context.Response.WriteAsync(wrongAnswEx.Message);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
