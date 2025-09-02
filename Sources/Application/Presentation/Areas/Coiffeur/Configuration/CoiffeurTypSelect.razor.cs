using JassApp.Domain.Coiffeur.Models;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.Configuration
{
    public partial class CoiffeurTypSelect
    {
        [Parameter]
        [EditorRequired]
        public required EventCallback<CoiffeurSpielrundeTyp> OnSelectedTypChanged { get; set; }

        private CoiffeurSpielrundeTyp SelectedTyp { get; set; } = CoiffeurSpielrundeTyp.WithGschobna;

        private async Task HandleTypChangedAsync(CoiffeurSpielrundeTyp typ)
        {
            SelectedTyp = typ;
            await OnSelectedTypChanged.InvokeAsync(typ);
        }
    }
}