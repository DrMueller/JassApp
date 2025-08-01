using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.Configuration
{
    public partial class TeamConfiguration
    {
        [Parameter]
        [EditorRequired]
        public required IReadOnlyCollection<Domain.Models.Spieler> Spieler { get; set; }
    }
}
