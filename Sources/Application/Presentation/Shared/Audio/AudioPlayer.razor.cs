using JassApp.Presentation.Infrastructure.JavaScript.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace JassApp.Presentation.Shared.Audio
{
    public partial class AudioPlayer
    {
        private ElementReference _audioRef;
        private bool _isInitialized;

        private bool _isPlaying;
        private IJSObjectReference? _module;

        [Inject]
        public required IJavaScriptLocator JsLocator { get; set; }

        [Inject]
        public required IJSRuntime JsRuntime { get; set; }

        [Parameter]
        [EditorRequired]
        public required string Src { get; set; }

        private bool Disabled => !_isInitialized || string.IsNullOrWhiteSpace(Src);

        private string VoiceIcon => _isPlaying ? Icons.Material.Filled.MicOff : Icons.Material.Filled.MicNone;

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (_module is not null)
                {
                    await _module.DisposeAsync();
                }
            }
            catch
            {
                // Dispose should not throw during circuit teardown.
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await AssureJavascriptModuleAsync();
                _isInitialized = true;
            }
        }

        private async Task AssureJavascriptModuleAsync()
        {
            var jsFilePath = await JsLocator.LocateJsFilePathAsync(this);
            _module ??= await JsRuntime.InvokeAsync<IJSObjectReference>("import", jsFilePath);
        }

        private async Task PlayAsync()
        {
            await _module!.InvokeVoidAsync("play", _audioRef);

            _isPlaying = true;
            StateHasChanged();
        }

        private async Task StopAsync()
        {
            await _module!.InvokeVoidAsync("stop", _audioRef);

            _isPlaying = false;
            StateHasChanged();
        }

        private async Task ToggleAsync()
        {
            if (Disabled)
            {
                return;
            }

            if (_isPlaying)
            {
                await StopAsync();
                return;
            }

            await PlayAsync();
        }
    }
}