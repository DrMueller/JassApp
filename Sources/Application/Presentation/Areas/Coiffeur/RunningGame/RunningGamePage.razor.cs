using JassApp.DataAccess.Repositories;
using JassApp.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class RunningGamePage
    {
        [Parameter]
        [EditorRequired]
        public required int GameId { get; set; }

        [Inject]
        public required ICoiffeurSpielrundeRepository RundeRepo { get; set; }

        private CoiffeurSpielrunde? Spielrunde { get; set; }

        private bool IsLoading => Spielrunde == null;

        public const string Path = "coiffeur/game/{gameId:int}";

        protected override async Task OnInitializedAsync()
        {
            Spielrunde = await RundeRepo.LoadAsync(new CoiffeurSpielrundeId(GameId));
        }
    }
}
