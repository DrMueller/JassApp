using JassApp.Presentation.Infrastructure.JavaScript.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace JassApp.Presentation.Shared.Voice
{
    public partial class Voice
    {
        private IJSObjectReference? _module;

        [Inject]
        public required IJavaScriptLocator JsLocator { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; } = default!;

        private string? ErrorMessage { get; set; }

        public async Task PrimeAsync(string lang = "de-CH")
        {
            try
            {
                ErrorMessage = null;
                await AssureJavascriptModuleAsync();
                await _module!.InvokeAsync<bool>("prime", lang);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                StateHasChanged();
                throw;
            }
        }

        public async Task SpeakAsync(string text, string lang = "de-CH")
        {
            try
            {
                ErrorMessage = null;
                await AssureJavascriptModuleAsync();
                await _module!.InvokeVoidAsync("speak", text, lang);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                StateHasChanged();
                throw;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    await AssureJavascriptModuleAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                    StateHasChanged();
                }
            }
        }

        private async Task AssureJavascriptModuleAsync()
        {
            var jsFilePath = await JsLocator.LocateJsFilePathAsync<Voice>();
            _module ??= await JsRuntime.InvokeAsync<IJSObjectReference>("import", jsFilePath);
        }
    }
}