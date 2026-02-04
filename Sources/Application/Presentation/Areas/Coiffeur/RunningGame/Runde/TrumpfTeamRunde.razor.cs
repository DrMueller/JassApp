using JassApp.Domain.Coiffeur.Models;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame.Runde
{
    public partial class TrumpfTeamRunde
    {
        [Parameter]
        [EditorRequired]
        public required CoiffeurTrumpfrunde CoiffeurTrumpfrunde { get; set; }

        [Parameter]
        [EditorRequired]
        public EventCallback OnValueChanged { get; set; }

        [Parameter]
        [EditorRequired]
        public required TrumpfrundeResultat TrumpfrundeResultat { get; set; }

        private async Task HandleValueChangedAsync()
        {
            await OnValueChanged.InvokeAsync();
        }
    }
}