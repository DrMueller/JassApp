using JassApp.Domain.Coiffeur.Models;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.SpielerHistory
{
    public partial class SpielerHistoryPage
    {
        [Parameter]
        [EditorRequired]
        public required int PlayerId { get; set; }

        private CoiffeurSpielrunde? Spielrunde { get; set; }

        private bool IsLoading => Spielrunde == null;

        public const string Path = "coiffeur/game/{gameId:int}";

    }
}
