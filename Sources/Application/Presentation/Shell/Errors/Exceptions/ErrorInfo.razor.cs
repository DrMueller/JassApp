using JassApp.Presentation.Areas.Home;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Shell.Errors.Exceptions
{
    [UsedImplicitly]
    public partial class ErrorInfo
    {
        [Parameter]
        [EditorRequired]
        public AppError? AppError { get; set; }

        [Inject]
        public required NavigationManager Navigator { get; set; }

        private void GoHome()
        {
            Navigator.NavigateTo(HomePage.Path, true);
        }
    }
}