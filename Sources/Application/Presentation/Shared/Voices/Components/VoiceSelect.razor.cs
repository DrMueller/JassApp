using JassApp.Presentation.Shared.Voices.Services;
using Microsoft.AspNetCore.Components;
using Toolbelt.Blazor.SpeechSynthesis;

namespace JassApp.Presentation.Shared.Voices.Components
{
    public partial class VoiceSelect
    {
        public required SpeechSynthesisVoice SelectedVoice { get; set; }

        [Inject]
        public required SpeechSynthesis SpeechSynthesis { get; set; }

        [Inject]
        public required IVoicePersister VoicePersister { get; set; }

        private bool IsLoading => Voices == null;

        private IReadOnlyCollection<SpeechSynthesisVoice>?
            Voices { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadVoicesAsync();
        }

        private async Task HandleVoiceChangedAsync(SpeechSynthesisVoice arg)
        {
            SelectedVoice = arg;
            await VoicePersister.SaveAsync(arg);
        }

        private async Task LoadVoicesAsync()
        {
            var voices = await SpeechSynthesis.GetVoicesAsync();
            SelectedVoice = await VoicePersister.LoadAsync();

            Voices = voices.OrderBy(f => f.Name).ToList();
        }
    }
}