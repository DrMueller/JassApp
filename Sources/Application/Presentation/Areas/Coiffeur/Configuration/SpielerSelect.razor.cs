using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.Configuration
{
    public partial class SpielerSelect
    {
        public Domain.Models.Spieler? SelectedSpieler { get; set; }

        [Parameter]
        [EditorRequired]
        public required IReadOnlyCollection<Domain.Models.Spieler> Spieler { get; set; }
    }
}
