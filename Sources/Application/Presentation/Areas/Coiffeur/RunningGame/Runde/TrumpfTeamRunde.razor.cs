using JassApp.Domain.Coiffeur.Models;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame.Runde
{
    public partial class TrumpfTeamRunde
    {
        [Parameter]
        [EditorRequired]
        public EventCallback OnValueChanged { get; set; }

        [Parameter]
        [EditorRequired]
        public required JassTeamTyp TeamTyp { get; set; }

        [Parameter]
        [EditorRequired]
        public required CoiffeurTrumpfrunde CoiffeurTrumpfrunde { get; set; }

        private string? Value { get; set; }

        protected override void OnInitialized()
        {
            Value = CoiffeurTrumpfrunde[TeamTyp].RawInput;
        }

        private async Task HandleValueChangedAsync(string arg)
        {
            Value = arg;
            CoiffeurTrumpfrunde[TeamTyp].RawInput = arg;
            await OnValueChanged.InvokeAsync();
        }
    }
}