using JassApp.Domain.Coiffeur.Models;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame.Runde
{
    public partial class TrumpfRundeDisplay
    {
        [Parameter]
        [EditorRequired]
        public EventCallback OnValueChanged { get; set; }

        [Parameter]
        [EditorRequired]
        public required CoiffeurTrumpfrunde CoiffeurTrumpfrunde { get; set; }
    }
}
