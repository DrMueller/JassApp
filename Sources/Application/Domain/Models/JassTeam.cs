using JassApp.Common.LanguageExtensions.Invariance;
using JetBrains.Annotations;

namespace JassApp.Domain.Models
{
    public record JassTeamSpieler(SpielerId Id, string Name);

    public record JassTeamÎd(int Value);

    [PublicAPI]
    public record JassTeam
    {
        public static JassTeam CreateNew(
            JassTeamSpieler spieler1, 
            JassTeamSpieler spieler2)
        {
            return new JassTeam(new JassTeamÎd(0), spieler1, spieler2);
        }

        public JassTeam(
            JassTeamÎd Id,
            JassTeamSpieler spieler1, 
            JassTeamSpieler spieler2)
        {
            Guard.ObjectNotNull(() => spieler1);
            Guard.ObjectNotNull(() => spieler2);
            Guard.That(() => spieler1 != spieler2, "Spieler1 and Spieler2 must be different players.");

            this.Id = Id;
            Spieler1 = spieler1;
            Spieler2 = spieler2;
        }

        public JassTeamÎd Id { get; }
        public JassTeamSpieler Spieler1 { get; }
        public JassTeamSpieler Spieler2 { get; }
    }
}