namespace JassApp.Domain.Coiffeur.Models
{
    public record CoiffeurSpielrundeOptionen(
        bool DoIncludeRaucherpausen, 
        bool DoIncludeShots,
        bool IsJassTrainingslager)
    {
        public string Description => $"Raucherpausen: {(DoIncludeRaucherpausen ? "Ja" : "Nein")}, Shots: {(DoIncludeShots ? "Ja" : "Nein")}, JTL: {(IsJassTrainingslager ? "Ja" : "Nein")}";
    }
}