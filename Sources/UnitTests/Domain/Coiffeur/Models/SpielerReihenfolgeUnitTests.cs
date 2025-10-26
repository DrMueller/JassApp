using FluentAssertions;
using JassApp.Domain.Coiffeur.Models;
using JassApp.UnitTests.TestingInfrastructure.DomainModelBuilders;
using Xunit;

namespace JassApp.UnitTests.Domain.Coiffeur.Models
{
    public class SpielerReihenfolgeUnitTests
    {
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 4, 0)]
        [InlineData(1, 5, 2)]
        [InlineData(1, 0, 1)]
        public void CalculatingActiveSpieler_CalculatesActiveSpieler(int startSpielerPos, int amountOfRoundsPlayed, int expectedActiveSpielerPos)
        {
            // Arrange
            var (team1, team2) = JassTeamTestBuilder.Create(startSpielerPos);
            var flatPlayers = new[]
            {
                team1.Spieler1,
                team2.Spieler1,
                team1.Spieler2,
                team2.Spieler2
            };

            var sut = new SpielerReihenfolge([team1, team2]);

            // Act
            var actualActiveSpieler = sut.CalculateActiveSpieler(amountOfRoundsPlayed);

            // Assert
            var expectedActiveSpieler = flatPlayers.ElementAt(expectedActiveSpielerPos);
            actualActiveSpieler.Should().Be(expectedActiveSpieler);
        }
    }
}