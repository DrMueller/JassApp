using FluentAssertions;
using JassApp.Domain.Coiffeur.Models;
using JassApp.UnitTests.TestingInfrastructure.DomainModelBuilders;
using Xunit;

namespace JassApp.UnitTests.Domain.Coiffeur.Models
{
    public class CoiffeurSpielrundeUnitTests
    {
        [Fact]
        public void CalculateTotalPunkte_ReturnsOwnSumAndDifference()
        {
            var (team1, team2) = JassTeamTestBuilder.Create();

            var runden = new[]
            {
                new CoiffeurTrumpfrunde(new TrumpfrundeId(1), 1, CoiffeurTrumpf.Herz,
                    new TrumpfrundeResultat(JassTeamTyp.Team1, 100, false, false),
                    new TrumpfrundeResultat(JassTeamTyp.Team2, 80, false, false)),
                new CoiffeurTrumpfrunde(new TrumpfrundeId(2), 2, CoiffeurTrumpf.Egge,
                    new TrumpfrundeResultat(JassTeamTyp.Team1, 120, false, false),
                    new TrumpfrundeResultat(JassTeamTyp.Team2, 100, false, false))
            };

            var sut = new CoiffeurSpielrunde(
                new CoiffeurSpielrundeId(1),
                DateTime.Today,
                10,
                runden,
                new[] { team1, team2 },
                new CoiffeurSpielrundeOptionen(false, false));

            var total = sut.CalculateTotalPunkte(JassTeamTyp.Team1);
            total.Punkte.Should().Be(60); // (100-80)*1 + (120-100)*2
            total.DifferenzZuGegner.Should().Be(60);
        }

        [Fact]
        public void CheckShouldSmoke_ReturnsFalse_WhenDisabled()
        {
            var (team1, team2) = JassTeamTestBuilder.Create();
            var runden = new[] { new CoiffeurTrumpfrunde(new TrumpfrundeId(1), 1, CoiffeurTrumpf.Herz) };

            var sut = new CoiffeurSpielrunde(
                new CoiffeurSpielrundeId(1),
                DateTime.Today,
                10,
                runden,
                new[] { team1, team2 },
                new CoiffeurSpielrundeOptionen(false, true));

            sut.CheckShouldSmoke().Should().BeFalse();
        }

        [Fact]
        public void PunktwertDescription_IsFormatted()
        {
            var (team1, team2) = JassTeamTestBuilder.Create();
            var runden = new[]
            {
                new CoiffeurTrumpfrunde(new TrumpfrundeId(1), 1, CoiffeurTrumpf.Herz,
                    new TrumpfrundeResultat(JassTeamTyp.Team1, 10, false, false),
                    new TrumpfrundeResultat(JassTeamTyp.Team2, 0, false, false))
            };

            var sut = new CoiffeurSpielrunde(
                new CoiffeurSpielrundeId(1),
                DateTime.Today,
                10,
                runden,
                new[] { team1, team2 },
                new CoiffeurSpielrundeOptionen(false, false));

            sut.PunktwertDescription.Should().Be("10 Rp.");
        }
    }
}