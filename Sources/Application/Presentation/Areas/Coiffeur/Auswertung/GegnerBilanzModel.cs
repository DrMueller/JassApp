namespace JassApp.Presentation.Areas.Coiffeur.Auswertung
{
    public record GegnerBilanzModel(
        string Gegner,
        int AnzahlSpielrunden,
        int PunkteSpieler,
        int PunkteGegner,
        int PunkteDifferenz,
        int MaetscheSpieler,
        int MaetscheGegner,
        int MaetschDifferenz);
}
