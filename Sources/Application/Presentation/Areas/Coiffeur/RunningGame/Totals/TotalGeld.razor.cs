using JassApp.Domain.Coiffeur.Models;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame.Totals
{
    public partial class TotalGeld
    {
        [Parameter]
        [EditorRequired]
        public required CoiffeurSpielrunde Spielrunde { get; set; }
    }
}
