namespace JassApp.Presentation.Areas.Coiffeur.Auswertung
{
    public record SpielrundenAuswertungErgebnisModel(
        IReadOnlyCollection<SpielrundenDetailModel> Details,
        AuswertungSummaryModel Summary,
        IReadOnlyCollection<GegnerBilanzModel> GegnerBilanzen)
    {
        public static SpielrundenAuswertungErgebnisModel Empty { get; } = new(
            [],
            AuswertungSummaryModel.Empty,
            []);
    }
}