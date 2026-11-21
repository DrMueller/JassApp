namespace JassApp.Domain.Coiffeur.Models
{
    public class TrumpfrundeResultat(JassTeamTyp teamTyp, int? punkte, bool istMatsch, bool istKontermatsch)
    {
        public TrumpfrundeResultat(JassTeamTyp teamTyp) : this(teamTyp, null, false, false)
        {
        }

        public bool IstGespielt => Punkte != null;

        public bool IstKontermatsch
        {
            get;
            set
            {
                if (Punkte != 0)
                {
                    field = false;
                    return;
                }

                field = value;
            }
        } = istKontermatsch;

        public bool IstMatch
        {
            get;
            set
            {
                if (Punkte != MaxPunkte!.MaxPunkte)
                {
                    field = false;
                    return;
                }

                field = value;
            }
        } = istMatsch;

        public TrumpfrundeMaximumPunkte? MaxPunkte { get; private set; }

        public int? Punkte
        {
            get;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (value > MaxPunkte!.MaxPunkte)
                {
                    value = MaxPunkte.MaxPunkte;
                }

                field = value;
            }
        } = punkte;

        public JassTeamTyp TeamTyp { get; } = teamTyp;

        public void InitializeMaxPunkte(TrumpfrundeMaximumPunkte maxPunkte)
        {
            MaxPunkte = maxPunkte;
        }
    }
}