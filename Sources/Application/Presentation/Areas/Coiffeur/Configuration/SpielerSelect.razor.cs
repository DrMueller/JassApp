using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.Configuration
{
    public partial class SpielerSelect
    {
        [Parameter]
        [EditorRequired]
        public required EventCallback<Domain.Models.Spieler> OnSpielerChanged { get; set; }

        [Parameter]
        [EditorRequired]
        public required IReadOnlyCollection<Domain.Models.Spieler> Spieler { get; set; }

        private Domain.Models.Spieler? SelectedSpieler { get; set; }

        private async Task HandleSpielerChangedAsync(Domain.Models.Spieler arg)
        {
            SelectedSpieler = arg;
            await OnSpielerChanged.InvokeAsync(arg);
        }
    }
}