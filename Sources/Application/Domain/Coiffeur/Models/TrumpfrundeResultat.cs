namespace JassApp.Domain.Coiffeur.Models
{
    public class TrumpfrundeResultat(JassTeamTyp teamTyp, int? punkte, bool istMatsch, bool istKontermatsch)
    {
        public TrumpfrundeResultat(JassTeamTyp teamTyp) : this(teamTyp, null, false, false)
        {
        }

        public bool IstGespielt => Punkte != null;
        public bool IstKonterMatch { get; set; } = istKontermatsch;
        public bool IstMatch { get; set; } = istMatsch;
        public int? Punkte { get; set; } = punkte;
        public JassTeamTyp TeamTyp { get; } = teamTyp;
    }
}