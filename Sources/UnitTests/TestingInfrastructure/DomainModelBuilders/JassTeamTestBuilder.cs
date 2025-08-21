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
            var team1 = new JassTeam(
                new JassTeamÎd(1),
                new JassTeamSpieler(new JassTeamSpielerId(1), new SpielerId(1), "Test1", false),
                new JassTeamSpieler(new JassTeamSpielerId(2), new SpielerId(2), "Tes2", true));

            var team2 = new JassTeam(
                new JassTeamÎd(2),
                new JassTeamSpieler(new JassTeamSpielerId(3), new SpielerId(3), "Test3", false),
                new JassTeamSpieler(new JassTeamSpielerId(4), new SpielerId(4), "Test4", true));

            return (team1, team2);
        }
    }
}