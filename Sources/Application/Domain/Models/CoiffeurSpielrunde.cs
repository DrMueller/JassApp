using JassApp.Common.LanguageExtensions.Invariance;
using JetBrains.Annotations;

namespace JassApp.Domain.Models
{
    public record CoiffeurSpielrundeId(int Value);

    [PublicAPI]
    public class CoiffeurSpielrunde
    {
        public CoiffeurSpielrunde(
            CoiffeurSpielrundeId id,
            int punkteWert,
            IReadOnlyCollection<Trumpfrunde> trumpfrunden,
            JassTeam team1,
            JassTeam team2)
        {
            Guard.ValueNotDefault(() => punkteWert);
            Guard.CollectionNotNullOrEmpty(() => trumpfrunden);
            Guard.ObjectNotNull(() => team1);
            Guard.ObjectNotNull(() => team2);

            Id = id;
            PunkteWert = punkteWert;
            Trumpfrunden = trumpfrunden;
            Team1 = team1;
            Team2 = team2;
        }

        public CoiffeurSpielrundeId Id { get; }
        public int PunkteWert { get; }
        public JassTeam Team1 { get; }
        public JassTeam Team2 { get; }
        public IReadOnlyCollection<Trumpfrunde> Trumpfrunden { get; }

        public void UpdateResultat(
            Trumpfrunde runde,
            JassTeam team,
            int punkte,
            bool istMatch)
        {
            var forTeam1 = team == Team1;
            Trumpfrunden.Single(f => f.Trumpf.Typ == runde.Trumpf.Typ)
                .UpdateResultat(forTeam1,
                    punkte,
                    istMatch);
        }
    }
}