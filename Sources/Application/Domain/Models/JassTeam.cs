using JassApp.Common.LanguageExtensions.Invariance;
using JetBrains.Annotations;

namespace JassApp.Domain.Models
{
    public record JassTeamSpieler(SpielerId Id, string Name);

    [PublicAPI]
    public record JassTeam
    {
        public JassTeam(JassTeamSpieler spieler1, JassTeamSpieler spieler2)
        {
            Guard.ObjectNotNull(() => spieler1);
            Guard.ObjectNotNull(() => spieler2);
            Guard.That(() => spieler1 != spieler2, "Spieler1 and Spieler2 must be different players.");

            Spieler1 = spieler1;
            Spieler2 = spieler2;
        }

        public JassTeamSpieler Spieler1 { get; }
        public JassTeamSpieler Spieler2 { get; }
    }
}