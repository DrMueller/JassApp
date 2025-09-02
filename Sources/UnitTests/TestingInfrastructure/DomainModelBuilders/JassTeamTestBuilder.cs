using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Spieler.Models;
using JetBrains.Annotations;

namespace JassApp.UnitTests.TestingInfrastructure.DomainModelBuilders
{
    [PublicAPI]
    public static class JassTeamTestBuilder
    {
        public static(JassTeam Team1, JassTeam Team2) Create()
        {
            var team1Spieler1 = new JassTeamSpieler(
                new JassTeamSpielerId(1),
                new SpielerId(1),
                "Test1",
                false,
                JassTeamSpielerPosition.Spieler1);

            var team1Spieler2 = new JassTeamSpieler(
                new JassTeamSpielerId(2),
                new SpielerId(2),
                "Tes2",
                true,
                JassTeamSpielerPosition.Spieler2);

            var team1 = new JassTeam(
                new JassTeamId(1),
                JassTeamTyp.Team1,
                [team1Spieler1, team1Spieler2]);

            var team2Spieler1 = new JassTeamSpieler(
                new JassTeamSpielerId(3),
                new SpielerId(3),
                "Test3",
                false,
                JassTeamSpielerPosition.Spieler1);

            var team2Spieler2 = new JassTeamSpieler(
                new JassTeamSpielerId(4),
                new SpielerId(4),
                "Test4",
                true,
                JassTeamSpielerPosition.Spieler2);

            var team2 = new JassTeam(
                new JassTeamId(2),
                JassTeamTyp.Team2,
                [team2Spieler1, team2Spieler2]);

            return (team1, team2);
        }
    }
}