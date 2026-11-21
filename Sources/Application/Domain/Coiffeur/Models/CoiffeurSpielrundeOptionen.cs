namespace JassApp.Domain.Coiffeur.Models
{
    public record CoiffeurSpielrundeOptionen(
        bool IsJassTrainingslager,
        bool HasRoundedPunkte)
    {
        public string Description => $"JTL: {(IsJassTrainingslager ? "Ja" : "Nein")}, Gerundete Punkte: {(HasRoundedPunkte ? "Ja" : "Nein")}";
    }
}