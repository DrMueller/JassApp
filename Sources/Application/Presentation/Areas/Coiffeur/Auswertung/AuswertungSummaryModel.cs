namespace JassApp.Presentation.Areas.Coiffeur.Auswertung
{
    public record AuswertungSummaryModel(
        int Anzahl,
        int PunkteSpieler,
        int PunkteGegner,
        int PunkteDifferenz,
        int MaetscheSpieler,
        int MaetscheGegner,
        int MaetschDifferenz)
    {
        public static AuswertungSummaryModel Empty { get; } = new(0, 0, 0, 0, 0, 0, 0);
    }
}
