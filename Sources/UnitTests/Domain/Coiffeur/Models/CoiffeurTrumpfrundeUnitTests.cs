using FluentAssertions;
using JassApp.Domain.Coiffeur.Models;
using Xunit;

namespace JassApp.UnitTests.Domain.Coiffeur.Models
{
    public class CoiffeurTrumpfrundeUnitTests
    {
        [Fact]
        public void Ctor_CreatesTwoEmptyResultate_ByDefault()
        {
            var sut = new CoiffeurTrumpfrunde(new TrumpfrundeId(1), 2, CoiffeurTrumpf.Herz);

            sut.AmountOfResultate.Should().Be(0);
            sut[JassTeamTyp.Team1].Punkte.Should().BeNull();
            sut[JassTeamTyp.Team2].Punkte.Should().BeNull();
        }

        [Fact]
        public void CalculatePunktedifferenz_WhenAnyResultMissing_ReturnsNull()
        {
            var sut = new CoiffeurTrumpfrunde(new TrumpfrundeId(1), 2, CoiffeurTrumpf.Herz);
            sut[JassTeamTyp.Team1].Punkte = 100;
            sut[JassTeamTyp.Team2].Punkte = null;

            sut.CalculatePunktedifferenz(JassTeamTyp.Team1).Should().BeNull();
        }

        [Theory]
        [InlineData(100, 50, 2, 100)] // (100-50)*2
        [InlineData(50, 100, 2, 0)]
        [InlineData(100, 100, 2, 0)]
        public void CalculatePunktedifferenz_ReturnsExpected(int team1, int team2, int mod, int expected)
        {
            var sut = new CoiffeurTrumpfrunde(new TrumpfrundeId(1), mod, CoiffeurTrumpf.Herz);
            sut[JassTeamTyp.Team1].Punkte = team1;
            sut[JassTeamTyp.Team2].Punkte = team2;

            sut.CalculatePunktedifferenz(JassTeamTyp.Team1).Should().Be(expected);
        }

        [Fact]
        public void CheckIstGewinner_WhenAnyResultMissing_ReturnsNull()
        {
            var sut = new CoiffeurTrumpfrunde(new TrumpfrundeId(1), 1, CoiffeurTrumpf.Herz);
            sut[JassTeamTyp.Team1].Punkte = 100;

            sut.CheckIstGewinner(JassTeamTyp.Team1).Should().BeNull();
        }

        [Theory]
        [InlineData(101, 100, true)]
        [InlineData(100, 101, false)]
        [InlineData(100, 100, false)]
        public void CheckIstGewinner_ReturnsExpected(int team1, int team2, bool expected)
        {
            var sut = new CoiffeurTrumpfrunde(new TrumpfrundeId(1), 1, CoiffeurTrumpf.Herz);
            sut[JassTeamTyp.Team1].Punkte = team1;
            sut[JassTeamTyp.Team2].Punkte = team2;

            sut.CheckIstGewinner(JassTeamTyp.Team1).Should().Be(expected);
        }
    }
}
