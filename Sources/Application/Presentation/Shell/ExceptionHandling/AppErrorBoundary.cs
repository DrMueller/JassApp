using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace JassApp.Presentation.Shell.ExceptionHandling
{
    [UsedImplicitly]
    public class AppErrorBoundary : ErrorBoundary
    {
        [Parameter]
        public EventCallback<Exception> OnExceptionThrown { get; set; }

        protected override async Task OnErrorAsync(Exception exception)
        {
            await OnExceptionThrown.InvokeAsync(exception);
            await base.OnErrorAsync(exception);
        }
    }
}