using JassApp.Domain.Coiffeur.Models;
using JassApp.Presentation.Shared.Voices;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class TeamDetails
    {
        [Parameter]
        [EditorRequired]
        public required JassTeamTyp TeamTyp { get; set; }

        [Parameter]
        [EditorRequired]
        public required CoiffeurSpielrunde Spielrunde { get; set; }

        private Voice VoiceRef { get; set; } = null!;

        private string TeamDescription => Spielrunde.GetTeamDescription(TeamTyp);

        public async Task HandleOpenTruempfeRequestedAsync()
        {
            var offeneTruempfe = Spielrunde.GetOffeneTruempfeDescription(TeamTyp);
            
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            VoiceRef.PrimeSpeechAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            await VoiceRef.SpeakAsync(offeneTruempfe);
        }
    }
}