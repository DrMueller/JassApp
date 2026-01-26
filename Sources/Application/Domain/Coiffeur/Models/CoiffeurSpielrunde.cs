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
            IReadOnlyCollection<JassTeam> jassTeams)
        {
            Guard.ValueNotDefault(() => punkteWert);
            Guard.CollectionNotNullOrEmpty(() => trumpfrunden);
            Guard.ObjectNotNull(() => jassTeams);

            Id = id;
            GestartetAm = gestartetAm;
            PunkteWert = punkteWert;
            Trumpfrunden = trumpfrunden;
            JassTeams = jassTeams;
        }

        public DateTime GestartetAm { get; }
        public CoiffeurSpielrundeId Id { get; }

        public JassTeam JassTeam1 => JassTeams.Single(f => f.Typ == JassTeamTyp.Team1);
        public JassTeam JassTeam2 => JassTeams.Single(f => f.Typ == JassTeamTyp.Team2);
        public IReadOnlyCollection<JassTeam> JassTeams { get; }
        public int PunkteWert { get; }
        public string PunktwertDescription => $"{PunkteWert} Rp.";
        public IReadOnlyCollection<CoiffeurTrumpfrunde> Trumpfrunden { get; }

        public int? CalculateMaetche(JassTeamTyp teamTyp)
        {
            var ownMaetsche = Trumpfrunden.Count(f => f[teamTyp].IstMatch);
            var opposingTeam = GetOpposingTeamType(teamTyp);
            var konterMaetsche = Trumpfrunden.Count(f => f[opposingTeam].IstKonterMatch);
            var istHerzGewinner = Trumpfrunden
                .Single(f => f.CoiffeurTrumpf.Typ == CoiffeurTrumpfTyp.Herz)
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

        public string GetTeamDescription(JassTeamTyp teamTyp)
        {
            var team = JassTeams.Single(f => f.Typ == teamTyp);

            var playedRounds = Trumpfrunden
                .Sum(f => f.AmountOfResultate);

            var reihenfolge = new SpielerReihenfolge(JassTeams);
            var activeSpieler = reihenfolge.CalculateActiveSpieler(playedRounds);

            return team.GetRundeDescription(activeSpieler);
        }

        public IReadOnlyCollection<string> GetOffeneTruempfe(JassTeamTyp team)
        {
            return Trumpfrunden
                .Where(f => !f[team].IstGespielt)
                .Select(f => f.CoiffeurTrumpf)
                .OrderBy(f => f.Typ)
                .Select(f => f.Name)
                .ToList();
        }

        private JassTeamTyp GetOpposingTeamType(JassTeamTyp teamTyp)
        {
            return teamTyp == JassTeamTyp.Team1 ? JassTeamTyp.Team2 : JassTeamTyp.Team1;
        }
    }
}