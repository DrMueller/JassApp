using JassApp.Common.LanguageExtensions.Invariance;
using JassApp.Common.LanguageExtensions.Types.Maybes;
using JassApp.Common.LanguageExtensions.Types.Maybes.Implementation;

namespace JassApp.Domain.Models
{
    public class Trumpfrunde
    {
        public Trumpfrunde(
            int punkteModifikator,
            Trumpf trumpf)
        {
            Guard.ValueNotDefault(() => punkteModifikator);

            PunkteModifikator = punkteModifikator;
            Trumpf = trumpf;
        }

        public Maybe<double> PunkteDifferenz { get; set; } = None.Value;
        public int PunkteModifikator { get; }

        public Maybe<TrumpfrundeResultat> ResultatTeam1 { get; set; } = None.Value;
        public Maybe<TrumpfrundeResultat> ResultatTeam2 { get; set; } = None.Value;
        public Trumpf Trumpf { get; }

        public void UpdateResultat(
            bool forTeam1,
            int punkte,
            bool istMatch)
        {
            if (forTeam1)
            {
                ResultatTeam1 = new TrumpfrundeResultat(punkte, istMatch);
            }
            else
            {
                ResultatTeam2 = new TrumpfrundeResultat(punkte, istMatch);
            }
        }
    }
}