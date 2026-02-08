using System.Net;

namespace JassApp.Presentation.Shell.Errors.NotFound
{
    public class NotFoundMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            await next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                context.Response.Redirect(NotFoundPage.Path);
            }
        }
    }
}