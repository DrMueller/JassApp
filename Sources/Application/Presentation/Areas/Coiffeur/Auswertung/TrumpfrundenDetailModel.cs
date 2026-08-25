using JassApp.Domain.Coiffeur.Models;

namespace JassApp.Presentation.Areas.Coiffeur.Auswertung
{
    public record TrumpfrundenDetailModel(
        DateTime GestartetAm,
        CoiffeurTrumpfTyp TrumpfrundeTyp,
        string SpielerTeam,
        string GegnerTeam,
        int PunkteSpieler,
        int PunkteGegner,
        int PunkteDifferenz);
}
