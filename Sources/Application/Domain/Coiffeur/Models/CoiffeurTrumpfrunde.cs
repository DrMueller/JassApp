using JassApp.Common.LanguageExtensions.Invariance;

namespace JassApp.Domain.Coiffeur.Models
{
    public record TrumpfrundeId(int Value);

    public class CoiffeurTrumpfrunde
    {
        private readonly IReadOnlyCollection<TrumpfrundeResultat> _resultate;

        public CoiffeurTrumpfrunde(
            TrumpfrundeId id,
            int punkteModifikator,
            CoiffeurTrumpf coiffeurTrumpf,
            TrumpfrundeResultat resultatTeam1,
            TrumpfrundeResultat resultatTeam2) : this(id, punkteModifikator, coiffeurTrumpf)
        {
            _resultate =
            [
                resultatTeam1,
                resultatTeam2
            ];
        }

        public CoiffeurTrumpfrunde(
            TrumpfrundeId id,
            int punkteModifikator,
            CoiffeurTrumpf coiffeurTrumpf)
        {
            Guard.ValueNotDefault(() => punkteModifikator);
            ID = id;

            _resultate = new List<TrumpfrundeResultat>
            {
                new(JassTeamTyp.Team1),
                new(JassTeamTyp.Team2)
            };

            PunkteModifikator = punkteModifikator;
            CoiffeurTrumpf = coiffeurTrumpf;
        }

        public int AmountOfResultate => _resultate.Count(f => f.Punkte.HasValue);

        public CoiffeurTrumpf CoiffeurTrumpf { get; }

        public string Description =>
            $"{CoiffeurTrumpf.Name} ({PunkteModifikator}x)";

        public TrumpfrundeId ID { get; }

        public int PunkteModifikator { get; }

        public TrumpfrundeResultat this[JassTeamTyp teamTyp] => _resultate.Single(f => f.TeamTyp == teamTyp);

        public int? CalculatePunktedifferenz(JassTeamTyp teamTyp)
        {
            var resultTeam1 = this[JassTeamTyp.Team1].Punkte;
            var resultTeam2 = this[JassTeamTyp.Team2].Punkte;

            if (resultTeam1 == null || resultTeam2 == null)
            {
                return null;
            }

            var rootTeamValue = teamTyp == JassTeamTyp.Team1
                ? resultTeam1
                : resultTeam2;

            var opposingTeamValue = teamTyp == JassTeamTyp.Team1
                ? resultTeam2
                : resultTeam1;

            var diff = rootTeamValue - opposingTeamValue;
            if (diff <= 0)
            {
                return 0;
            }

            return diff * PunkteModifikator;
        }

        public bool? CheckIstGewinner(JassTeamTyp teamTyp)
        {
            var resultTeam1 = this[JassTeamTyp.Team1].Punkte;
            var resultTeam2 = this[JassTeamTyp.Team2].Punkte;

            if (resultTeam1 == null || resultTeam2 == null)
            {
                return null;
            }

            var rootTeamValue = teamTyp == JassTeamTyp.Team1
                ? resultTeam1
                : resultTeam2;

            var opposingTeamValue = teamTyp == JassTeamTyp.Team1
                ? resultTeam2
                : resultTeam1;

            return rootTeamValue > opposingTeamValue;
        }
    }
}