using JassApp.Domain.Spieler.BusinessObjects;
using JassApp.Domain.Spieler.Services;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.SpielerHistory
{
    public partial class SpielerHistoryPage
    {
        public const string Path = "coiffeur/game/playerhistory";

        [Inject]
        public required ISpielerQueryService QueryService { get; set; }

        private IReadOnlyCollection<SpielerHistoryEntryBo>? Entries { get; set; }

        private bool IsLoading => Entries == null;

        protected override async Task OnInitializedAsync()
        {
            Entries = await QueryService.LoadHistoryAsync();
        }
    }
}