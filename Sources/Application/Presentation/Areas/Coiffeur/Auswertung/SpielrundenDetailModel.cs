namespace JassApp.Presentation.Areas.Coiffeur.Auswertung
{
    public record SpielrundenDetailModel(
        DateTime GestartetAm,
        string SpielerTeam,
        string GegnerTeam,
        int PunkteSpieler,
        int PunkteGegner,
        int PunkteDifferenz,
        int MaetscheSpieler,
        int MaetscheGegner,
        int MaetschDifferenz);
}
