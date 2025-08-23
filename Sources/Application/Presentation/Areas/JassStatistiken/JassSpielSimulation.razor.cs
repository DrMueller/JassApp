using JassApp.Domain.Jassrunden.Models;
using JassApp.Domain.Jassrunden.Models.Jass;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.JassStatistiken
{
    public partial class JassSpielSimulation
    {
        [Parameter]
        [EditorRequired]
        public required JassSpielrunde Runde { get; set; }
    }
}
