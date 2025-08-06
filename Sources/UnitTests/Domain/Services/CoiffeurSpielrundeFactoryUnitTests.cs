using FluentAssertions;
using JassApp.Domain.Models;
using JassApp.Domain.Services.Implementation;
using JassApp.Domain.Services.Servants;
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
            var spieler = SpielerTestBuilder.Create();
            const CoiffeurSpielrundeTyp typ = CoiffeurSpielrundeTyp.WithDifferenzler;

            const int punkteWert = 10;

            var trumpfrunde = new Trumpfrunde(1, Trumpf.Differenzler);

            _trumpfRundenFactoryMock
                .Setup(f => f.Create(typ))
                .Returns([trumpfrunde]);

            // Act
            var actualSpielRundeResult = _sut.TryCreating(
                10,
                typ,
                spieler.Spieler1,
                spieler.Spieler2,
                spieler.Spieler3,
                spieler.Spieler4);

            // Assert
            var actualSpielRunde = actualSpielRundeResult.ShouldBeRight();
            actualSpielRunde.Team1.Spieler1.Id.Should().Be(spieler.Spieler1.Id);
            actualSpielRunde.Team1.Spieler2.Id.Should().Be(spieler.Spieler2.Id);
            actualSpielRunde.Team2.Spieler1.Id.Should().Be(spieler.Spieler3.Id);
            actualSpielRunde.Team2.Spieler2.Id.Should().Be(spieler.Spieler4.Id);

            actualSpielRunde.PunkteWert.Should().Be(punkteWert);

            actualSpielRunde.Trumpfrunden.Should().HaveCount(1);
            actualSpielRunde.Trumpfrunden.Single().Should().Be(trumpfrunde);
        }

        [Fact]
        public void TryCreating_WithSameSpieler_ReturnsError()
        {
            // Arrange
            const int duplicateSpielerId = 1;

            // Act
            var actualResult = _sut.TryCreating(1,
                CoiffeurSpielrundeTyp.WithGschobna,
                new Spieler(new SpielerId(duplicateSpielerId), "Spieler1", []),
                new Spieler(new SpielerId(duplicateSpielerId), "Spieler21", []),
                new Spieler(new SpielerId(3), "Spieler3", []),
                new Spieler(new SpielerId(4), "Spieler4", []));

            // Assert
            var actualInfoEntries = actualResult.ShouldBeLeft();
            actualInfoEntries.ErrorMessages.Single().Should().Be(CoiffeurSpielrundeFactory.SpielerNichtEindeutigErrorMessage);
        }
    }
}