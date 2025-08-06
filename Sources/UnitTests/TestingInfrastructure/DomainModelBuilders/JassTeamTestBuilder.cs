using JassApp.Domain.Models;
using JetBrains.Annotations;

namespace JassApp.UnitTests.TestingInfrastructure.DomainModelBuilders
{
    [PublicAPI]
    public static class JassTeamTestBuilder
    {
        public static(JassTeam Team1, JassTeam Team2) Create()
        {
            var team1 = new JassTeam(
                new JassTeamÎd(1),
                new JassTeamSpieler(new SpielerId(1), "Test1"),
                new JassTeamSpieler(new SpielerId(2), "Tes2"));

            var team2 = new JassTeam(
                new JassTeamÎd(2),
                new JassTeamSpieler(new SpielerId(3), "Test3"),
                new JassTeamSpieler(new SpielerId(4), "Test4"));

            return (team1, team2);
        }
    }
}