using JassApp.Domain.Coiffeur.Models;

namespace JassApp.UnitTests.TestingInfrastructure.DomainModelBuilders
{
    internal static class CoiffeurSpielrundeTestBuilder
    {
        internal static CoiffeurSpielrunde Create()
        {
            var trumpfrunden = new List<CoiffeurTrumpfrunde>
            {
                new(new TrumpfrundeId(0), 1, CoiffeurTrumpf.Herz),
                new(new TrumpfrundeId(1), 1, CoiffeurTrumpf.Egge)
            };

            var (team1, team2) = JassTeamTestBuilder.Create();

            return new CoiffeurSpielrunde(
                new CoiffeurSpielrundeId(0),
                DateTime.Now,
                10,
                trumpfrunden,
                [team1, team2],
                new CoiffeurSpielrundeOptionen(true, true));
        }
    }
}