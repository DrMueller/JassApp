using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Coiffeur.Repositories;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class RunningGamePage
    {
        public const string Path = "coiffeur/game/{gameId:int}";

        [Parameter]
        [EditorRequired]
        public required int GameId { get; set; }

        [Inject]
        public required ICoiffeurSpielrundeRepository RundeRepo { get; set; }

        private bool IsLoading => Spielrunde == null;

        private CoiffeurSpielrunde? Spielrunde { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Spielrunde = await RundeRepo.LoadAsync(new CoiffeurSpielrundeId(GameId));
        }
    }
}