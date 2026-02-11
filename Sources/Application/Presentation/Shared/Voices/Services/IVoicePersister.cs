using Toolbelt.Blazor.SpeechSynthesis;

namespace JassApp.Presentation.Shared.Voices.Services
{
    public interface IVoicePersister
    {
        Task<SpeechSynthesisVoice> LoadAsync();
        Task SaveAsync(SpeechSynthesisVoice voice);
    }
}
