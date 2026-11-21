namespace JassApp.Domain.Coiffeur.Models
{
    public abstract class TrumpfrundeMaximumPunkte
    {
        public abstract int MaxPunkte { get; }
    }

    public class UnroundedMaxPunkte : TrumpfrundeMaximumPunkte
    {
        public override int MaxPunkte => 157;
    }

    public class RoundedMaxPunkte : TrumpfrundeMaximumPunkte
    {
        public override int MaxPunkte => 16;
    }
}
