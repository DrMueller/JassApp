using JassApp.Presentation.Shared.Voices.Services;
using Microsoft.AspNetCore.Components;
using Toolbelt.Blazor.SpeechSynthesis;

namespace JassApp.Presentation.Shared.Voices.Components
{
    public partial class Voice
    {
        [Inject]
        public required SpeechSynthesis SpeechSynthesis { get; set; }

        [Inject]
        public required IVoicePersister VoicePersister { get; set; }

        public async Task SpeakAsync(string text)
        {
            var voice = await VoicePersister.LoadAsync();

            var utterancet = new SpeechSynthesisUtterance
            {
                Text = text,
                Voice = voice
            };

            await SpeechSynthesis.SpeakAsync(utterancet);
        }
    }
}