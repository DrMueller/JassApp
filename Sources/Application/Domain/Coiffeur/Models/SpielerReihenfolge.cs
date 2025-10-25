namespace JassApp.Domain.Coiffeur.Models
{
    public class SpielerReihenfolge(
        JassTeamSpieler spieler1,
        JassTeamSpieler spieler2,
        JassTeamSpieler spieler3,
        JassTeamSpieler spieler4)
    {
        public JassTeamSpieler CalculateActiveSpieler(int amountOfRundenPlayed)
        {
            var position = (spieler1.Position switch
            {
                JassTeamSpielerPosition.Spieler1 => 0,
                JassTeamSpielerPosition.Spieler2 => 1,
                _ => throw new InvalidOperationException("Unknown player position.")
            } + amountOfRundenPlayed) % 4;
            return position switch
            {
                0 => spieler1,
                1 => spieler2,
                2 => spieler3,
                3 => spieler4,
                _ => throw new InvalidOperationException("Invalid calculation for active player.")
            };
        }
    }
}
