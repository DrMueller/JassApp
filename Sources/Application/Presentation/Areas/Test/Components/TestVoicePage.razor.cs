using JassApp.Presentation.Shared.Voices;
using Microsoft.AspNetCore.Components;
using Toolbelt.Blazor.SpeechSynthesis;

namespace JassApp.Presentation.Areas.Test.Components
{
    public partial class TestVoicePage
    {
        private const string Path = "/test/voice";

        [Inject]
        public required SpeechSynthesis SpeechSynthesis { get; set; }

        private Voice VoiceRef { get; set; } = null!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await Task.Run(async () =>
            {
                Thread.Sleep(2000);
                await VoiceRef.SpeakAsync("Hallo Welt");
            });
        }

        private async Task TestVoiceAsync()
        {
            await SpeechSynthesis.SpeakAsync("Hallo Welt from Toolbelt");
        }
    }
}