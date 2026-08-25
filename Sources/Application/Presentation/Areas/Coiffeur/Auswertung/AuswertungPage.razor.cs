using JassApp.Common.InformationHandling;
using JassApp.Domain.Coiffeur.Models;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.Auswertung
{
    public partial class AuswertungPage
    {
        public const string Path = "coiffeur/auswertung";

        [Inject]
        public required IAuswertungService AuswertungService { get; set; }

        private InformationEntries? Infos { get; set; }

        private bool IsLoading { get; set; } = true;
        private bool IsRunning { get; set; }

        private SpielrundenAuswertungErgebnisModel SpielrundenErgebnis { get; set; } = SpielrundenAuswertungErgebnisModel.Empty;
        private IReadOnlyCollection<AuswertungSpielerItemModel> Spieler { get; set; } = [];
        private AuswertungFilterModel Filter { get; } = new();
        private TrumpfrundenAuswertungErgebnisModel TrumpfrundenErgebnis { get; set; } = TrumpfrundenAuswertungErgebnisModel.Empty;

        protected override async Task OnInitializedAsync()
        {
            var result = await AuswertungService.LoadSpielerAsync();
            Infos = result.Infos;
            Spieler = result.Spieler;
            IsLoading = false;
        }

        private async Task AuswertenAsync()
        {
            IsRunning = true;
            Infos = null;

            var spielrundenResult = await AuswertungService.EvaluateSpielrundenAsync(Filter);
            Infos = spielrundenResult.Infos;
            SpielrundenErgebnis = spielrundenResult.Ergebnis;

            if (Infos.HasErrorsOrWarnings)
            {
                TrumpfrundenErgebnis = TrumpfrundenAuswertungErgebnisModel.Empty;
                IsRunning = false;
                return;
            }

            var trumpfrundenResult = await AuswertungService.EvaluateTrumpfrundenAsync(Filter);
            Infos = Infos?.MergeWith(trumpfrundenResult.Infos) ?? trumpfrundenResult.Infos;
            TrumpfrundenErgebnis = trumpfrundenResult.Ergebnis;

            IsRunning = false;
        }

        private static List<CoiffeurTrumpfTyp> LoadTrumpfTypes()
        {
            return Enum.GetValues<CoiffeurTrumpfTyp>()
                .OrderBy(f => (int)f)
                .ToList();
        }
    }
}
