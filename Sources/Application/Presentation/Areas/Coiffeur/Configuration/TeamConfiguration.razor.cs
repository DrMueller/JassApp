using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.Configuration
{
    public partial class TeamConfiguration
    {
        [Parameter]
        [EditorRequired]
        public EventCallback<Domain.Models.Spieler> OnSpieler1Selected { get; set; }

        [Parameter]
        [EditorRequired]
        public EventCallback<Domain.Models.Spieler> OnSpieler2Selected { get; set; }

        [Parameter]
        [EditorRequired]
        public EventCallback<Domain.Models.Spieler> OnSpieler3Selected { get; set; }

        [Parameter]
        [EditorRequired]
        public EventCallback<Domain.Models.Spieler> OnSpieler4Selected { get; set; }

        [Parameter]
        [EditorRequired]
        public required IReadOnlyCollection<Domain.Models.Spieler> Spieler { get; set; }
    }
}