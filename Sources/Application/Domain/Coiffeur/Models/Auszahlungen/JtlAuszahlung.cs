namespace JassApp.Domain.Coiffeur.Models.Auszahlungen
{
    public class JtlAuszahlung(CoiffeurSpielrundeAuszahlungFuerTeam team1, CoiffeurSpielrundeAuszahlungFuerTeam team2, int punkteWertInRappen)
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

                var punkte1DiffTeam1 = Math.Max(Team1.TotalPunkte.DifferenzZuGegner!.Value, 0);

                var punkte1DiffTeam2 = Math.Max(Team2.TotalPunkte.DifferenzZuGegner!.Value, 0);

                var franken1 = new Franken(punkte1DiffTeam1,
                    Team2.Maetsche * 2,
                    PunkteWertInRappen);

                var franken2 = new Franken(punkte1DiffTeam2,
                    Team1.Maetsche * 2,
                    PunkteWertInRappen);

                return $"Team 1: {franken1.Description}, Team 2: {franken2.Description}";
            }
        }
    }
}