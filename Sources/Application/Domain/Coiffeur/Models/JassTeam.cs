using JassApp.Common.LanguageExtensions.Invariance;
using JassApp.Domain.Spieler.Models;
using JetBrains.Annotations;

namespace JassApp.Domain.Coiffeur.Models
{
    public enum JassTeamSpielerPosition
    {
        Spieler1 = 1,
        Spieler2 = 2
    }

    public record JassTeamSpielerId(int Value);

    public record JassTeamSpieler(
        JassTeamSpielerId Id,
        SpielerId SpielerId,
        string Name,
        bool IstStartSpieler,
        JassTeamSpielerPosition Position);

    [PublicAPI]
    public record JassTeamId(int Value);

    [PublicAPI]
    public record JassTeam
    {
        private readonly IReadOnlyCollection<JassTeamSpieler> _spieler;

        public JassTeam(
            JassTeamId id,
            JassTeamTyp typ,
            IReadOnlyCollection<JassTeamSpieler> spieler)
        {
            Guard.That(() => spieler.Count() == 2, "Genau 2 Spieler pro Team");
            Guard.That(() => spieler.ElementAt(0) != spieler.ElementAt(1), "Spieler1 and Spieler2 must be different players.");
            _spieler = spieler;

            Id = id;
            Typ = typ;
        }

        public string Description => $"{Spieler1.Name} & {Spieler2.Name}";

        public JassTeamId Id { get; }
        public JassTeamSpieler Spieler1 => _spieler.Single(f => f.Position == JassTeamSpielerPosition.Spieler1);
        public JassTeamSpieler Spieler2 => _spieler.Single(f => f.Position == JassTeamSpielerPosition.Spieler2);
        public JassTeamTyp Typ { get; }

        public static JassTeam CreateNew(
            IReadOnlyCollection<JassTeamSpieler> jassTeamSpieler,
            JassTeamTyp typ)
        {
            return new JassTeam(
                new JassTeamId(0),
                typ,
                jassTeamSpieler);
        }

        public string GetRundeDescription(JassTeamSpieler activeSpieler)
        {
            var spieler1Name = Spieler1.IstStartSpieler ? $"{Spieler1.Name}*" : Spieler1.Name;
            var spieler2Name = Spieler2.IstStartSpieler ? $"{Spieler2.Name}*" : Spieler2.Name;

            if (activeSpieler.Id == Spieler1.Id)
            {
                spieler1Name = $"[{spieler1Name}]";
            }
            else if (activeSpieler.Id == Spieler2.Id)
            {
                spieler2Name = $"[{spieler2Name}]";
            }

            return $"{spieler1Name} & {spieler2Name}";
        }
    }
}