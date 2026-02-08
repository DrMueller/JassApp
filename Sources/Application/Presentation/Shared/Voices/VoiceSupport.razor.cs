using JassApp.Presentation.Infrastructure.JavaScript.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace JassApp.Presentation.Shared.Voices
{
    public partial class VoiceSupport
    {
        private IJSObjectReference? _module;

        [Inject]
        public required IJavaScriptLocator JsLocator { get; set; }

        [Inject]
        public required IJSRuntime JsRuntime { get; set; }

        private bool IsLoading => _module == null;

        private bool IsSupported { get; set; }

        private Color IsSupportedColor
        {
            get
            {
                if (IsSupported)
                {
                    return Color.Success;
                }

                return Color.Error;
            }
        }

        private string IsSupportedIcon
        {
            get
            {
                if (IsSupported)
                {
                    return Icons.Material.Filled.RecordVoiceOver;
                }

                return Icons.Material.Filled.VoiceOverOff;
            }
        }

        public async Task CheckSupportAsync()
        {
            if (IsLoading)
            {
                return;
            }

            IsSupported = await _module!.InvokeAsync<bool>("checkVoiceSupport");
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await AssureJavascriptModuleAsync();
                await CheckSupportAsync();
            }
        }

        private async Task AssureJavascriptModuleAsync()
        {
            var jsFilePath = await JsLocator.LocateJsFilePathAsync(this);
            _module ??= await JsRuntime.InvokeAsync<IJSObjectReference>("import", jsFilePath);
        }
    }
}