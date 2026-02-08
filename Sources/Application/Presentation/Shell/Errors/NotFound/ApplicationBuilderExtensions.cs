namespace JassApp.Presentation.Shell.Errors.NotFound
{
    internal static class ApplicationBuilderExtensions
    {
        public static void UseNotFoundRedirect(this IApplicationBuilder app)
        {
            app.UseMiddleware<NotFoundMiddleware>();
        }
    }
}