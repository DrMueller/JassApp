namespace JassApp.Domain.Coiffeur.Models.Auszahlungen
{
    public record CoiffeurSpielrundeAuszahlungFuerTeam(
        Punktetotal? TotalPunkte,
        int Maetsche);

    public abstract class CoiffeurSpielrundeAuszahlung(
        CoiffeurSpielrundeAuszahlungFuerTeam team1,
        CoiffeurSpielrundeAuszahlungFuerTeam team2,
        int punkteWertInRappen)
    {
        public abstract string Description { get; }
        protected int PunkteWertInRappen { get; } = punkteWertInRappen;

        protected CoiffeurSpielrundeAuszahlungFuerTeam Team1 { get; } = team1;
        protected CoiffeurSpielrundeAuszahlungFuerTeam Team2 { get; } = team2;

        public static CoiffeurSpielrundeAuszahlung Create(CoiffeurSpielrundeAuszahlungFuerTeam team1, CoiffeurSpielrundeAuszahlungFuerTeam team2,
            int punkteWertInRappen, bool isJtl)
        {
            if (isJtl)
            {
                return new JtlAuszahlung(team1, team2, punkteWertInRappen);
            }

            return new NonJtlAuszahlung(team1, team2, punkteWertInRappen);
        }
    }
}