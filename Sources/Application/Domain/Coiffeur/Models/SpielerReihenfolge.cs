namespace JassApp.Domain.Coiffeur.Models
{
    public class SpielerReihenfolge
    {
        private readonly IDictionary<int, JassTeamSpieler> _sortedSpieler;

        public SpielerReihenfolge(IReadOnlyCollection<JassTeam> teams)
        {
            _sortedSpieler = new Dictionary<int, JassTeamSpieler>
            {
                { 0, teams.Single(t => t.Typ == JassTeamTyp.Team1).Spieler1 },
                { 1, teams.Single(t => t.Typ == JassTeamTyp.Team2).Spieler1 },
                { 2, teams.Single(t => t.Typ == JassTeamTyp.Team1).Spieler2 },
                { 3, teams.Single(t => t.Typ == JassTeamTyp.Team2).Spieler2 },
            };
        }

        public JassTeamSpieler CalculateActiveSpieler(int amountOfRundenPlayed)
        {
            var startIndex = _sortedSpieler.Values.ToList().FindIndex(s => s.IstStartSpieler);

            return _sortedSpieler[(startIndex + amountOfRundenPlayed) % 4];
        }
    }
}
