using JassApp.Presentation.Shared.Voices;

namespace JassApp.Presentation.Areas.Test.Components
{
    public partial class TestVoicePage
    {
        private const string Path = "/test/voice";

        private Voice VoiceRef { get; set; } = null!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await Task.Run(async () =>
            {
                Thread.Sleep(2000);
                await VoiceRef.SpeakAsync("Hallo Welt");
            });
        }
    }
}