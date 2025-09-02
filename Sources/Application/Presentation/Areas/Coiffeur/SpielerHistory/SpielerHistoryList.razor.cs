using JassApp.Domain.Spieler.Specifications;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.SpielerHistory
{
    public partial class SpielerHistoryList
    {
        [Parameter]
        [EditorRequired]
        public required IReadOnlyCollection<SpielerHistoryEntryBo> Entries { get; set; }
    }
}