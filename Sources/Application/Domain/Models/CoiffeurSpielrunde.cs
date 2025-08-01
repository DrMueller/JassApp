using JassApp.Common.LanguageExtensions.Invariance;
using JetBrains.Annotations;

namespace JassApp.Domain.Models
{
    [PublicAPI]
    public class CoiffeurSpielrunde
    {
        private readonly int _punkteWert;

        public CoiffeurSpielrunde(
            int punkteWert,
            IReadOnlyCollection<Trumpfrunde> runden,
            JassTeam team1,
            JassTeam team2)
        {
            Guard.ValueNotDefault(() => punkteWert);
            Guard.CollectionNotNullOrEmpty(() => runden);
            Guard.ObjectNotNull(() => team1);
            Guard.ObjectNotNull(() => team2);

            Runden = runden;
            Team1 = team1;
            Team2 = team2;
            _punkteWert = punkteWert;
        }

        public IReadOnlyCollection<Trumpfrunde> Runden { get; }
        public JassTeam Team1 { get; }
        public JassTeam Team2 { get; }

        public void UpdateResultat(
            Trumpfrunde runde,
            JassTeam team,
            int punkte,
            bool istMatch)
        {
            var forTeam1 = team == Team1;
            Runden.Single(f => f.Trumpf.Typ == runde.Trumpf.Typ)
                .UpdateResultat(forTeam1,
                    punkte,
                    istMatch);
        }
    }
}