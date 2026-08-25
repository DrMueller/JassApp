namespace JassApp.Presentation.Areas.Coiffeur.Auswertung
{
    public record TrumpfrundenSummaryModel(
        int Anzahl,
        int PunkteSpieler,
        int PunkteGegner,
        int PunkteDifferenz)
    {
        public static TrumpfrundenSummaryModel Empty { get; } = new(0, 0, 0, 0);
    }
}
