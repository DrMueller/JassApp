using FluentAssertions;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Coiffeur.Services.Implementation;
using JassApp.Domain.Coiffeur.Services.Servants;
using JassApp.Domain.Spieler.Models;
using JassApp.UnitTests.TestingInfrastructure.DomainModelBuilders;
using JassApp.UnitTests.TestingInfrastructure.Extension;
using Moq;
using Xunit;

namespace JassApp.UnitTests.Domain.Services
{
    public class CoiffeurSpielrundeFactoryUnitTests
    {
        private readonly CoiffeurSpielrundeFactory _sut;
        private readonly Mock<ITrumpfRundenFactory> _trumpfRundenFactoryMock;

        public CoiffeurSpielrundeFactoryUnitTests()
        {
            _trumpfRundenFactoryMock = new Mock<ITrumpfRundenFactory>();
            _sut = new CoiffeurSpielrundeFactory(
                _trumpfRundenFactoryMock.Object);
        }

        [Fact]
        public void TryCreating_CreatesCoiffeurSpielrunde()
        {
            // Arrange
            var (spieler1, spieler2, spieler3, spieler4) = SpielerTestBuilder.Create();
            const CoiffeurSpielrundeTyp typ = CoiffeurSpielrundeTyp.WithDifferenzler;

            const int punkteWert = 10;

            var trumpfrunde = new CoiffeurTrumpfrunde(new TrumpfrundeId(1), 1, CoiffeurTrumpf.Differenzler);

            _trumpfRundenFactoryMock
                .Setup(f => f.Create(typ))
                .Returns([trumpfrunde]);

            // Act
            var actualSpielRundeResult = _sut.TryCreating(
                10,
                typ,
                spieler1,
                spieler2,
                spieler3,
                spieler4,
                spieler1,
                true,
                true);

            // Assert
            var actualSpielRunde = actualSpielRundeResult.ShouldBeRight();
            actualSpielRunde.JassTeam1.Spieler1.SpielerId.Should().Be(spieler1.Id);
            actualSpielRunde.JassTeam1.Spieler2.SpielerId.Should().Be(spieler2.Id);
            actualSpielRunde.JassTeam2.Spieler1.SpielerId.Should().Be(spieler3.Id);
            actualSpielRunde.JassTeam2.Spieler2.SpielerId.Should().Be(spieler4.Id);
            actualSpielRunde.PunkteWert.Should().Be(punkteWert);
            actualSpielRunde.Trumpfrunden.Should().HaveCount(1);
            actualSpielRunde.Trumpfrunden.Single().Should().Be(trumpfrunde);
        }

        [Fact]
        public void TryCreating_WithSameSpieler_ReturnsError()
        {
            // Arrange
            const int duplicateSpielerId = 1;

            var spieler3 = new Spieler(new SpielerId(3), "Spieler3", []);

            // Act
            var actualResult = _sut.TryCreating(1,
                CoiffeurSpielrundeTyp.WithGschobna,
                new Spieler(new SpielerId(duplicateSpielerId), "Spieler1", []),
                new Spieler(new SpielerId(duplicateSpielerId), "Spieler21", []),
                spieler3,
                new Spieler(new SpielerId(4), "Spieler4", []),
                spieler3,
                true,
                true);

            // Assert
            var actualInfoEntries = actualResult.ShouldBeLeft();
            actualInfoEntries.ErrorMessages.Single().Should().Be(CoiffeurSpielrundeFactory.SpielerNichtEindeutigErrorMessage);
        }
    }
}