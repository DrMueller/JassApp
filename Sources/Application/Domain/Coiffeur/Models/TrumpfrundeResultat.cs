namespace JassApp.Domain.Coiffeur.Models
{
    public class TrumpfrundeResultat(JassTeamTyp teamTyp, int? punkte, bool istMatsch, bool istKontermatsch)
    {
        private const int MaxPunkte = 16;
        private bool _istKontermatsch = istKontermatsch;
        private bool _istMatsch = istMatsch;

        public TrumpfrundeResultat(JassTeamTyp teamTyp) : this(teamTyp, null, false, false)
        {
        }

        public bool IstGespielt => Punkte != null;

        public bool IstKontermatsch
        {
            get => _istKontermatsch;
            set
            {
                if (Punkte != 0)
                {
                    _istKontermatsch = false;
                    return;
                }

                _istKontermatsch = value;
            }
        }

        public bool IstMatch
        {
            get => _istMatsch;
            set
            {
                if (Punkte != MaxPunkte)
                {
                    _istMatsch = false;
                    return;
                }

                _istMatsch = value;
            }
        }

        public int? Punkte { get; set; } = punkte;
        public JassTeamTyp TeamTyp { get; } = teamTyp;
    }
}