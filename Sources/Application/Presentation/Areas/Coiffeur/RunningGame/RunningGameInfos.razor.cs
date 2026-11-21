using JassApp.Domain.Coiffeur.Models;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class RunningGameInfos
    {
        [Parameter]
        [EditorRequired]
        public EventCallback OnValueChanged { get; set; }

        [Parameter]
        [EditorRequired]
        public required bool SpectatorMode { get; set; }

        [Parameter]
        [EditorRequired]
        public required CoiffeurSpielrunde Spielrunde { get; set; }

        private async Task HandleJassTrainingslagerChangedAsync(bool value)
        {
            Spielrunde.SetIsJassTrainingslager(value);
            await OnValueChanged.InvokeAsync();
        }
    }
}