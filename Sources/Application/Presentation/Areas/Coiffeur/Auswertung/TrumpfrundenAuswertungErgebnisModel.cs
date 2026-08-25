namespace JassApp.Presentation.Areas.Coiffeur.Auswertung
{
    public record TrumpfrundenAuswertungErgebnisModel(
        IReadOnlyCollection<TrumpfrundenDetailModel> Details,
        TrumpfrundenSummaryModel Summary)
    {
        public static TrumpfrundenAuswertungErgebnisModel Empty { get; } = new(
            [],
            TrumpfrundenSummaryModel.Empty);
    }
}