using JassApp.Common.LanguageExtensions.Invariance;
using JassApp.Common.LanguageExtensions.Types.Maybes;
using JassApp.Common.LanguageExtensions.Types.Maybes.Implementation;
using JetBrains.Annotations;

namespace JassApp.Domain.Coiffeur.Models
{
    public record CoiffeurSpielrundeId(int Value);

    [PublicAPI]
    public class CoiffeurSpielrunde
    {
        private int _shotsAfterNRounds;

        public CoiffeurSpielrunde(
            CoiffeurSpielrundeId id,
            DateTime gestartetAm,
            int punkteWert,
            IReadOnlyCollection<CoiffeurTrumpfrunde> trumpfrunden,
            IReadOnlyCollection<JassTeam> jassTeams,
            CoiffeurSpielrundeOptionen optionen)
        {
            Guard.ValueNotDefault(() => punkteWert);
            Guard.CollectionNotNullOrEmpty(() => trumpfrunden);
            Guard.ObjectNotNull(() => jassTeams);
            Guard.ObjectNotNull(() => optionen);

            Id = id;
            GestartetAm = gestartetAm;
            PunkteWert = punkteWert;
            Trumpfrunden = trumpfrunden;
            JassTeams = jassTeams;
            Optionen = optionen;

            _shotsAfterNRounds = Random.Shared.Next(10, 20);
        }

        public DateTime GestartetAm { get; }
        public CoiffeurSpielrundeId Id { get; }

        public JassTeam JassTeam1 => JassTeams.Single(f => f.Typ == JassTeamTyp.Team1);
        public JassTeam JassTeam2 => JassTeams.Single(f => f.Typ == JassTeamTyp.Team2);
        public IReadOnlyCollection<JassTeam> JassTeams { get; }
        public CoiffeurSpielrundeOptionen Optionen { get; }
        public int PunkteWert { get; }
        public string PunktwertDescription => $"{PunkteWert} Rp.";
        public IReadOnlyCollection<CoiffeurTrumpfrunde> Trumpfrunden { get; }

        public bool WasFinished => Trumpfrunden.All(f => f[JassTeamTyp.Team1].IstGespielt && f[JassTeamTyp.Team2].IstGespielt);

        public int? CalculateMaetche(JassTeamTyp teamTyp)
        {
            var ownMaetsche = Trumpfrunden.Count(f => f[teamTyp].IstMatch);
            var opposingTeam = GetOpposingTeamType(teamTyp);
            var konterMaetsche = Trumpfrunden.Count(f => f[opposingTeam].IstKontermatsch);
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

        public bool CheckShouldSmoke()
        {
            if (!Optionen.DoIncludeRaucherpausen)
            {
                return false;
            }

            var playedResultate = Trumpfrunden
                .Sum(f => f.AmountOfResultate);

            return playedResultate == 12;
        }

        public Maybe<string> CheckWhoShouldOrderShots()
        {
            var playedAmount = Trumpfrunden
                .Sum(f => f.AmountOfResultate);

            if (!Optionen.DoIncludeShots)
            {
                return None.Value;
            }

            var shouldOrder = playedAmount == _shotsAfterNRounds;
            if (shouldOrder)
            {
                var allPlayers = new List<string>
                {
                    JassTeam1.Spieler1.Name,
                    JassTeam2.Spieler2.Name,
                    JassTeam1.Spieler2.Name,
                    JassTeam2.Spieler1.Name
                };

                var randomPlayer = allPlayers[Random.Shared.Next(allPlayers.Count)];
                return randomPlayer;
            }

            return None.Value;
        }

        public string GetOffeneTruempfeDescription(JassTeamTyp team)
        {
            var offen = Trumpfrunden
                .Where(f => !f[team].IstGespielt)
                .Select(f => f.CoiffeurTrumpf)
                .OrderBy(f => f.Typ)
                .Select(f => f.Name)
                .ToList();

            if (offen.Count == 0)
            {
                return "Keine offenen Trümpfe";
            }

            if (offen.Count == Trumpfrunden.Count)
            {
                return "Alle Trümpfe offen";
            }

            if (offen.Count >= Trumpfrunden.Count - 3)
            {
                var gespielt = Trumpfrunden
                    .Where(f => f[team].IstGespielt)
                    .Select(f => f.CoiffeurTrumpf)
                    .OrderBy(f => f.Typ)
                    .Select(f => f.Name)
                    .ToList();

                return "Alles ausser " + string.Join(", ", gespielt);
            }

            return string.Join(", ", offen);
        }

        public string GetTeamDescription(JassTeamTyp teamTyp)
        {
            var orderAll = new List<(JassTeamTyp Team, JassTeamSpieler Spieler)>
            {
                (JassTeamTyp.Team1, JassTeam1.Spieler1),
                (JassTeamTyp.Team2, JassTeam2.Spieler1),
                (JassTeamTyp.Team1, JassTeam1.Spieler2),
                (JassTeamTyp.Team2, JassTeam2.Spieler2)
            };

            var startIndexAll = orderAll.FindIndex(s => s.Spieler.IstStartSpieler);
            if (startIndexAll < 0)
            {
                startIndexAll = 0;
            }

            // Pro eingetragene Runde ist der nächste Spieler dran.
            var playedTotal = Trumpfrunden.Sum(f => f.AmountOfResultate);

            // Basierend auf der 4er-Reihenfolge bestimmen wir den aktuellen Geber.
            // Damit gelten automatisch auch die "weiter zählen"-Beispiele beim Abschluss.
            var dealerIndexAll = (startIndexAll + playedTotal) % orderAll.Count;

            var team1Finished = Trumpfrunden.Count(f => f[JassTeamTyp.Team1].IstGespielt) == Trumpfrunden.Count;
            var team2Finished = Trumpfrunden.Count(f => f[JassTeamTyp.Team2].IstGespielt) == Trumpfrunden.Count;

            if (team1Finished ^ team2Finished)
            {
                var finishedTeam = team1Finished ? JassTeamTyp.Team1 : JassTeamTyp.Team2;

                while (orderAll[dealerIndexAll].Team != finishedTeam)
                {
                    dealerIndexAll = (dealerIndexAll + 1) % orderAll.Count;
                }
            }

            var dealer = orderAll[dealerIndexAll].Spieler;

            var team = JassTeams.Single(f => f.Typ == teamTyp);
            return team.GetRundeDescription(dealer);
        }

        private JassTeamTyp GetOpposingTeamType(JassTeamTyp teamTyp)
        {
            return teamTyp == JassTeamTyp.Team1 ? JassTeamTyp.Team2 : JassTeamTyp.Team1;
        }
    }
}