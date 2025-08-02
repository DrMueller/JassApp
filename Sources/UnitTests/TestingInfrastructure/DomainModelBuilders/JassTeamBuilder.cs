using JassApp.Domain.Models;

namespace JassApp.UnitTests.TestingInfrastructure.DomainModelBuilders
{
    public static class JassTeamBuilder
    {
        public static(JassTeam Team1, JassTeam Team2) Create()
        {
            var team1 = new JassTeam(
                new JassTeamSpieler(new SpielerId(1), "Test1"),
                new JassTeamSpieler(new SpielerId(2), "Tes2"));

            var team2 = new JassTeam(
                new JassTeamSpieler(new SpielerId(3), "Test3"),
                new JassTeamSpieler(new SpielerId(4), "Test4"));

            return (team1, team2);
        }
    }
}