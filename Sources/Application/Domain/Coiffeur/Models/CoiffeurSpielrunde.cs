using JassApp.Common.LanguageExtensions.Invariance;
using JetBrains.Annotations;

namespace JassApp.Domain.Coiffeur.Models
{
    public record CoiffeurSpielrundeId(int Value);

    [PublicAPI]
    public class CoiffeurSpielrunde
    {
        public CoiffeurSpielrunde(
            CoiffeurSpielrundeId id,
            DateTime gestartetAm,
            int punkteWert,
            IReadOnlyCollection<CoiffeurTrumpfrunde> trumpfrunden,
            JassTeam team1,
            JassTeam team2)
        {
            Guard.ValueNotDefault(() => punkteWert);
            Guard.CollectionNotNullOrEmpty(() => trumpfrunden);
            Guard.ObjectNotNull(() => team1);
            Guard.ObjectNotNull(() => team2);

            Id = id;
            GestartetAm = gestartetAm;
            PunkteWert = punkteWert;
            Trumpfrunden = trumpfrunden;
            Team1 = team1;
            Team2 = team2;
        }

        public DateTime GestartetAm { get; }
        public CoiffeurSpielrundeId Id { get; }
        public int PunkteWert { get; }
        public string PunktwertDescription => $"{PunkteWert} Rp.";
        public JassTeam Team1 { get; }
        public JassTeam Team2 { get; }
        public IReadOnlyCollection<CoiffeurTrumpfrunde> Trumpfrunden { get; }

        public int? CalculateMaetche(JassTeamTyp teamTyp)
        {
            var ownMaetsche =
                Trumpfrunden
                    .Count(f => f[teamTyp].IstMatch);

            var opposingTeam = GetOpposingTeamType(teamTyp);

            var konterMaetsche =
                Trumpfrunden
                    .Count(f => f[opposingTeam].IstKonterMatch);

            var istHerzGewinner =
                Trumpfrunden.Single(f => f.CoiffeurTrumpf.Typ == CoiffeurTrumpfTyp.Herz)
                    .CheckIstGewinner(teamTyp);

            if (istHerzGewinner == true)
            {
                ownMaetsche += 1;
            }

            return ownMaetsche + konterMaetsche * 2;
        }

        public Punktetotal CalculateTotalPunkte(JassTeamTyp teamTyp)
        {
            var ownPunkte =
                Trumpfrunden
                    .Select(tr => tr.CalculatePunktedifferenz(teamTyp))
                    .Sum();

            var opposingTeamType = GetOpposingTeamType(teamTyp);
            var opposingPunkte =
                Trumpfrunden
                    .Select(tr => tr.CalculatePunktedifferenz(opposingTeamType))
                    .Sum();

            return new Punktetotal(ownPunkte, ownPunkte - opposingPunkte);
        }

        private JassTeamTyp GetOpposingTeamType(JassTeamTyp teamTyp)
        {
            return teamTyp == JassTeamTyp.Team1 ? JassTeamTyp.Team2 : JassTeamTyp.Team1;
        }
    }
}