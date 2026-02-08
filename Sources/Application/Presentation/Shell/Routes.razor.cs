using JassApp.Presentation.Shell.Errors.Exceptions;

namespace JassApp.Presentation.Shell
{
    public partial class Routes
    {
        private AppError? AppError { get; set; }

        private AppErrorBoundary? ErrorBoundary { get; set; }

        protected override void OnParametersSet()
        {
            ErrorBoundary?.Recover();
        }

        private void HandleExceptionThrown(Exception arg)
        {
            AppError = new AppError(arg.GetType().Name, arg.Message, arg.StackTrace!);
        }
    }
}