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

        public async Task PrimeAsync(string lang = "de-CH")
        {
            await AssureJavascriptModuleAsync();
            await _module!.InvokeAsync<bool>("prime", lang);
        }

        public async Task SpeakAsync(IReadOnlyCollection<string> text, int delayMs = 0, string lang = "de-CH")
        {
            await AssureJavascriptModuleAsync();
            await _module!.InvokeVoidAsync("speakList", text, delayMs, lang);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await AssureJavascriptModuleAsync();
            }
        }

        private async Task AssureJavascriptModuleAsync()
        {
            var jsFilePath = await JsLocator.LocateJsFilePathAsync<Voice>();
            _module ??= await JsRuntime.InvokeAsync<IJSObjectReference>("import", jsFilePath);
        }
    }
}