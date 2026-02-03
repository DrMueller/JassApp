using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Coiffeur.Specifications;
using JassApp.Domain.Shared.Data.Querying;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class RunningGamePage
    {
        public const string Path = "coiffeur/game/{gameId:int}/{spectatorMode:boolean}";

        [Parameter]
        [EditorRequired]
        public required int GameId { get; set; }

        [Inject]
        public required IQueryService QueryService { get; set; }

        [Parameter]
        [EditorRequired]
        public required bool SpectatorMode { get; set; }

        private bool IsLoading => Spielrunde == null;

        private CoiffeurSpielrunde? Spielrunde { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var spec = new CoiffeurSpielrundeSpec(new CoiffeurSpielrundeId(GameId));
            Spielrunde = await QueryService.QuerySingleAsync(spec);
        }
    }
}