using Microsoft.AspNetCore.Components;
using Toolbelt.Blazor.SpeechSynthesis;

namespace JassApp.Presentation.Shared.Voice
{
    public partial class Voice
    {
        [Inject]
        public required SpeechSynthesis SpeechSynthesis { get; set; }

        public async Task SpeakAsync(string text)
        {
            var utterancet = new SpeechSynthesisUtterance
            {
                Text = text,
                Lang = "de-CH",
                Pitch = 1.5,
                Rate = 1.3,
                Volume = 1.0
            };

            await SpeechSynthesis.SpeakAsync(utterancet);
        }
    }
}