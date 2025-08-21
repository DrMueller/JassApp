namespace JassApp.Domain.Coiffeur.Models
{
    public record Ausbezahlung(double Geld)
    {
        public string Description => $"{Geld} CHF";
    }
}
