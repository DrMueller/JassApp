using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.Configuration
{
    public partial class TeamConfiguration
    {
        [Parameter]
        [EditorRequired]
        public EventCallback<Domain.Spieler.Models.Spieler> OnSpieler1Selected { get; set; }

        [Parameter]
        [EditorRequired]
        public EventCallback<Domain.Spieler.Models.Spieler> OnSpieler2Selected { get; set; }

        [Parameter]
        [EditorRequired]
        public EventCallback<Domain.Spieler.Models.Spieler> OnSpieler3Selected { get; set; }

        [Parameter]
        [EditorRequired]
        public EventCallback<Domain.Spieler.Models.Spieler> OnSpieler4Selected { get; set; }

        [Parameter]
        [EditorRequired]
        public required IReadOnlyCollection<Domain.Spieler.Models.Spieler> Spieler { get; set; }
    }
}