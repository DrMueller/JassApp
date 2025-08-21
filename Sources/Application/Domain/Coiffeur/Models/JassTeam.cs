using JassApp.Common.LanguageExtensions.Invariance;
using JassApp.Domain.Spieler.Models;
using JetBrains.Annotations;

namespace JassApp.Domain.Coiffeur.Models
{
    public record JassTeamSpielerId(int Value);

    public record JassTeamSpieler(
        JassTeamSpielerId Id,
        SpielerId SpielerId,
        string Name,
        bool IstStartSpieler);

    public record JassTeamÎd(int Value);

    [PublicAPI]
    public record JassTeam
    {
        public JassTeam(
            JassTeamÎd id,
            JassTeamSpieler spieler1,
            JassTeamSpieler spieler2)
        {
            Guard.ObjectNotNull(() => spieler1);
            Guard.ObjectNotNull(() => spieler2);
            Guard.That(() => spieler1 != spieler2, "Spieler1 and Spieler2 must be different players.");

            Id = id;
            Spieler1 = spieler1;
            Spieler2 = spieler2;
        }

        public string Description
        {
            get
            {
                var spieler1Name = Spieler1.IstStartSpieler ? $"{Spieler1.Name}*" : Spieler1.Name;
                var spieler2Name = Spieler2.IstStartSpieler ? $"{Spieler2.Name}*" : Spieler2.Name;

                return $"{spieler1Name} & {spieler2Name}";
            }
        }

        public JassTeamÎd Id { get; }
        public JassTeamSpieler Spieler1 { get; }
        public JassTeamSpieler Spieler2 { get; }

        public static JassTeam CreateNew(
            JassTeamSpieler spieler1,
            JassTeamSpieler spieler2)
        {
            return new JassTeam(new JassTeamÎd(0), spieler1, spieler2);
        }
    }
}