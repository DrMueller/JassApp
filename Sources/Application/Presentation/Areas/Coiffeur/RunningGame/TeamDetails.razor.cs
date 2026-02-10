using JassApp.Domain.Coiffeur.Models;
using Microsoft.AspNetCore.Components;
using Toolbelt.Blazor.SpeechSynthesis;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class TeamDetails
    {
        [Inject]
        public required SpeechSynthesis SpeechSynthesis { get; set; }

        [Parameter]
        [EditorRequired]
        public required CoiffeurSpielrunde Spielrunde { get; set; }

        [Parameter]
        [EditorRequired]
        public required JassTeamTyp TeamTyp { get; set; }

        private string TeamDescription => Spielrunde.GetTeamDescription(TeamTyp);

        //private Voice VoiceRef { get; set; } = null!;

        public async Task HandleOpenTruempfeRequestedAsync()
        {
            var offeneTruempfe = Spielrunde.GetOffeneTruempfeDescription(TeamTyp);

            await SpeechSynthesis.SpeakAsync(offeneTruempfe);
            //await VoiceRef.SpeakAsync(offeneTruempfe);
        }
    }
}