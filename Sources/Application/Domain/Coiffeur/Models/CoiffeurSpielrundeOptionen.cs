namespace JassApp.Domain.Coiffeur.Models
{
    public record CoiffeurSpielrundeOptionen(bool DoIncludeRaucherpausen, bool DoIncludeShots)
    {
        public string Description => $"Raucherpausen: {(DoIncludeRaucherpausen ? "Ja" : "Nein")}, Shots: {(DoIncludeShots ? "Ja" : "Nein")}";
    }
}