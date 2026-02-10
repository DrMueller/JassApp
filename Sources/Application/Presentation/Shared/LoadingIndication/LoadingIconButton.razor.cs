using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Shared.LoadingIndication
{
    public partial class LoadingIconButton
    {
        private bool _isLoading;

        [Parameter]
        [EditorRequired]
        public required EventCallback OnClick { get; set; }

        [Parameter]
        [EditorRequired]
        public required string Icon { get; set; }

        private async Task HandleClickAsync()
        {
            if (_isLoading)
            {
                return;
            }

            _isLoading = true;
            try
            {
                await OnClick.InvokeAsync();
            }
            finally
            {
                _isLoading = false;
            }
        }
    }
}