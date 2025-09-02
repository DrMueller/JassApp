using JassApp.Domain.Shared.Data.Querying;
using JassApp.Domain.Spieler.Specifications;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.SpielerHistory
{
    public partial class SpielerHistoryPage
    {
        public const string Path = "coiffeur/game/playerhistory";

        [Inject]
        public required IQueryService QueryService { get; set; }

        private IReadOnlyCollection<SpielerHistoryEntryBo>? Entries { get; set; }

        private bool IsLoading => Entries == null;

        protected override async Task OnInitializedAsync()
        {
            Entries = await QueryService.QueryAsync(new SpielerHistorySpec());
        }
    }
}