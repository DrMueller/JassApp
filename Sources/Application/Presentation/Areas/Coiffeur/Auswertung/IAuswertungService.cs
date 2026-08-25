using JassApp.Common.InformationHandling;

namespace JassApp.Presentation.Areas.Coiffeur.Auswertung
{
    public interface IAuswertungService
    {
        Task<(InformationEntries Infos, IReadOnlyCollection<AuswertungSpielerItemModel> Spieler)> LoadSpielerAsync();

        Task<(InformationEntries Infos, SpielrundenAuswertungErgebnisModel Ergebnis)> EvaluateSpielrundenAsync(AuswertungFilterModel filter);

        Task<(InformationEntries Infos, TrumpfrundenAuswertungErgebnisModel Ergebnis)> EvaluateTrumpfrundenAsync(AuswertungFilterModel filter);
    }
}
