using JassApp.Presentation.Infrastructure.JavaScript.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace JassApp.Presentation.Shared.Voices
{
    public partial class Voice
    {
        private IJSObjectReference? _module;

        [Inject]
        public required IJavaScriptLocator JsLocator { get; set; }

        [Inject]
        public required IJSRuntime JsRuntime { get; set; }

        public async Task SpeakAsync(string text)
        {
            await _module!.InvokeVoidAsync("speak", text);
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