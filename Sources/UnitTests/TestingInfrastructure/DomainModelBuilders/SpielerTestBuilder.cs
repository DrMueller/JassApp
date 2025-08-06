using JassApp.Domain.Models;

namespace JassApp.UnitTests.TestingInfrastructure.DomainModelBuilders
{
    public static class SpielerTestBuilder
    {
        public static(Spieler Spieler1, Spieler Spieler2, Spieler Spieler3, Spieler Spieler4) Create()
        {
            return (
                new Spieler(new SpielerId(1), "Spieler1", []),
                new Spieler(new SpielerId(2), "Spieler2", []),
                new Spieler(new SpielerId(3), "Spieler3", []),
                new Spieler(new SpielerId(4), "Spieler4", [])
            );
        }
    }
}