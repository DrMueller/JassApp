using JassApp.Domain.Coiffeur.Models;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class Spieltafel
    {
        [Parameter]
        [EditorRequired]
        public required CoiffeurSpielrunde Spielrunde { get; set; }

        private void HandleValueChanged()
        {
            StateHasChanged();
        }
    }
}
