using JassApp.Common.Logging.Services;
using JassApp.Presentation.Shell.Errors.Exceptions;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Shell
{
    public partial class Routes
    {
        [Inject]
        public ILoggingService LoggingService { get; set; } = null!;

        private AppError? AppError { get; set; }

        private AppErrorBoundary? ErrorBoundary { get; set; }

        protected override void OnParametersSet()
        {
            ErrorBoundary?.Recover();
        }

        private void HandleExceptionThrown(Exception arg)
        {
            LoggingService.LogException(arg);
            AppError = new AppError(arg.GetType().Name, arg.Message, arg.StackTrace!);
        }
    }
}