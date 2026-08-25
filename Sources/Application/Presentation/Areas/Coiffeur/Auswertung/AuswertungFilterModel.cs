using JassApp.Domain.Coiffeur.Models;

namespace JassApp.Presentation.Areas.Coiffeur.Auswertung
{
    public enum JasstrainingslagerFilterTyp
    {
        Alle = 0,
        Ja = 1,
        Nein = 2
    }

    public class AuswertungFilterModel
    {
        public int? GegnerId { get; set; }
        public JasstrainingslagerFilterTyp JasstrainingslagerFilter { get; set; } = JasstrainingslagerFilterTyp.Alle;
        public int? SpielerId { get; set; }
        public CoiffeurTrumpfTyp? TrumpfrundeTypFilter { get; set; }
        public DateTime? Von { get; set; }
        public DateTime? Bis { get; set; }
    }
}
