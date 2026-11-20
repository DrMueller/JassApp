namespace JassApp.Domain.Coiffeur.Models
{
    public record CoiffeurSpielrundeOptionen(
        bool IsJassTrainingslager)
    {
        public string Description => $"JTL: {(IsJassTrainingslager ? "Ja" : "Nein")}";
    }
}