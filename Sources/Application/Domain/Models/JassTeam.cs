using JassApp.Common.LanguageExtensions.Invariance;

namespace JassApp.Domain.Models
{
    public record JassTeam
    {
        public JassTeam(Spieler spieler1, Spieler spieler2)
        {
            Guard.ObjectNotNull(() => spieler1);
            Guard.ObjectNotNull(() => spieler2);
            Guard.That(() => spieler1 != spieler2, "Spieler1 and Spieler2 must be different players.");

            Spieler1 = spieler1;
            Spieler2 = spieler2;
        }

        public Spieler Spieler1 { get; }
        public Spieler Spieler2 { get; }
    }
}