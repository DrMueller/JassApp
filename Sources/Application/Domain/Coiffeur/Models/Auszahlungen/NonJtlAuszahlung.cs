namespace JassApp.Domain.Coiffeur.Models.Auszahlungen
{
    public class NonJtlAuszahlung(CoiffeurSpielrundeAuszahlungFuerTeam team1, CoiffeurSpielrundeAuszahlungFuerTeam team2, int punkteWertInRappen)
        : CoiffeurSpielrundeAuszahlung(team1, team2, punkteWertInRappen)
    {
        public override string Description
        {
            get
            {
                if (Team1.TotalPunkte == null || Team2.TotalPunkte == null)
                {
                    return string.Empty;
                }

                var franken1 = new Franken(Team1.TotalPunkte.DifferenzZuGegner!.Value, 0, PunkteWertInRappen);

                var franken2 = new Franken(Team2.TotalPunkte.DifferenzZuGegner!.Value, 0, PunkteWertInRappen);

                var team1HasToPay = Team1.TotalPunkte.DifferenzZuGegner!.Value < 0;

                var str = team1HasToPay ? $"Team 1: {franken2.Description}" : $"Team 2: {franken1.Description}";

                return str;
            }
        }
    }
}