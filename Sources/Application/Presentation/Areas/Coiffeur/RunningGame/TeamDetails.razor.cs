using JassApp.Domain.Coiffeur.Models;
using JassApp.Presentation.Shared.Voices;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class TeamDetails
    {
        [Parameter]
        [EditorRequired]
        public required CoiffeurSpielrunde Spielrunde { get; set; }

        [Parameter]
        [EditorRequired]
        public required JassTeamTyp TeamTyp { get; set; }

        private string TeamDescription => Spielrunde.GetTeamDescription(TeamTyp);

        private Voice VoiceRef { get; set; } = null!;

        public async Task HandleOpenTruempfeRequestedAsync()
        {
            var offeneTruempfe = Spielrunde.GetOffeneTruempfeDescription(TeamTyp);

            await VoiceRef.SpeakAsync(offeneTruempfe);
        }
    }
}