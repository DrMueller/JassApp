using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.Configuration
{
    public partial class SpielerSelect
    {
        [Parameter]
        [EditorRequired]
        public required EventCallback<Domain.Spieler.Models.Spieler> OnSpielerChanged { get; set; }

        [Parameter]
        [EditorRequired]
        public required IReadOnlyCollection<Domain.Spieler.Models.Spieler> Spieler { get; set; }

        [Parameter]
        public required string Label { get; set; } = "Spieler";

        private Domain.Spieler.Models.Spieler? SelectedSpieler { get; set; }

        private async Task HandleSpielerChangedAsync(Domain.Spieler.Models.Spieler arg)
        {
            SelectedSpieler = arg;
            await OnSpielerChanged.InvokeAsync(arg);
        }
    }
}