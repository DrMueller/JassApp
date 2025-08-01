using FluentAssertions;
using JassApp.Domain.Models;
using JassApp.Domain.Services;
using JassApp.Domain.Services.Implementation;
using JassApp.UnitTests.TestingInfrastructure.DomainModelBuilders;
using Moq;
using Xunit;

namespace JassApp.UnitTests.Domain.Services
{
    public class CoiffeurSpielrundeFactoryUnitTests
    {
        private readonly CoiffeurSpielrundeFactory _sut;
        private readonly Mock<ITrumpfFactory> _trumpFactoryMock;

        public CoiffeurSpielrundeFactoryUnitTests()
        {
            _trumpFactoryMock = new Mock<ITrumpfFactory>();
            _sut = new CoiffeurSpielrundeFactory(_trumpFactoryMock.Object);
        }

        [Fact]
        public void CreatingSpielrunde_WithGschobna_AssignsCorrectValuesToEachTrumpf()
        {
            // Arrange
            var teams = JassTeamBuilder.Create();

            var drueliDrue = new Trumpf(TrumpfTyp.DrueliDrue, "t");
            var herz = new Trumpf(TrumpfTyp.Herz, "t");

            _trumpFactoryMock
                .Setup(f => f.CreateWithGschobna())
                .Returns([drueliDrue, herz]);

            // Act
            var actualRunde = _sut.CreateGschobna(
                10,
                teams.Team1,
                teams.Team2);

            // Assert
            actualRunde.Runden.Should().HaveCount(2);
            actualRunde.Runden.ElementAt(0).Trumpf.Typ.Should().Be(TrumpfTyp.Herz);
            actualRunde.Runden.ElementAt(0).PunkteModifikator.Should().Be(1);

            actualRunde.Runden.ElementAt(1).Trumpf.Typ.Should().Be(TrumpfTyp.DrueliDrue);
            actualRunde.Runden.ElementAt(1).PunkteModifikator.Should().Be(2);
        }
    }
}