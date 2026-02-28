using JassApp.Common.LanguageExtensions.Types.Maybes;
using JassApp.Presentation.Shared.Storage;
using Toolbelt.Blazor.SpeechSynthesis;

namespace JassApp.Presentation.Shared.Voices.Services.Implementation
{
    public class VoicePersister(SpeechSynthesis synth, ILocalStorageProxy storage) : IVoicePersister
    {
        private const string VoiceKey = "voice";

        public async Task<SpeechSynthesisVoice> LoadAsync()
        {
            var voices = await synth.GetVoicesAsync();

            return await storage
                .GetStringAsync(VoiceKey)
                .MapAsync(identity => voices.First(f => f.VoiceIdentity == identity))
                .ReduceAsync(() => GetDefaultVoice(voices));
        }

        public async Task SaveAsync(SpeechSynthesisVoice voice)
        {
            await storage.SetItemAsync(VoiceKey, voice.VoiceIdentity);
        }

        private static SpeechSynthesisVoice GetDefaultVoice(IReadOnlyCollection<SpeechSynthesisVoice> voices)
        {
            var swissVoice = voices.FirstOrDefault(f => f.Lang == "de-CH");

            if (swissVoice != null)
            {
                return swissVoice;
            }

            var germanVoice = voices.FirstOrDefault(f => f.Lang.ToLower().StartsWith("de-"));
            if (germanVoice != null)
            {
                return germanVoice;
            }

            return voices.First();
        }
    }
}