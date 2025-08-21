using FluentAssertions;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Coiffeur.Services.Servants.Implementation;
using Xunit;

namespace JassApp.UnitTests.Domain.Services.Servants
{
    public class TrumpfRundenFactoryUnitTests
    {
        private readonly TrumpfRundenFactory _sut;

        public TrumpfRundenFactoryUnitTests()
        {
            _sut = new TrumpfRundenFactory();
        }

        [Fact]
        public void Creating_MitDifferenzler_RemovesGschobna()
        {
            // Act
            var actualRunden = _sut.Create(CoiffeurSpielrundeTyp.WithDifferenzler);

            // Assert
            actualRunden.Should().HaveCount(13);
            actualRunden.Should().NotContain(r => r.CoiffeurTrumpf.Typ == CoiffeurTrumpfTyp.Gschobna);
            actualRunden.Should().Contain(r => r.CoiffeurTrumpf.Typ == CoiffeurTrumpfTyp.Differenzler);
        }

        [Fact]
        public void Creating_MitGschobna_RemovesDifferenzler()
        {
            // Act
            var actualRunden = _sut.Create(CoiffeurSpielrundeTyp.WithGschobna);

            // Assert
            actualRunden.Should().HaveCount(13);
            actualRunden.Should().Contain(r => r.CoiffeurTrumpf.Typ == CoiffeurTrumpfTyp.Gschobna);
            actualRunden.Should().NotContain(r => r.CoiffeurTrumpf.Typ == CoiffeurTrumpfTyp.Differenzler);
        }

        [Fact]
        public void Creating_OhneBeides_RemovesGschobna_AndDifferenzler()
        {
            // Act
            var actualRunden = _sut.Create(CoiffeurSpielrundeTyp.OhneBeides);

            // Assert
            actualRunden.Should().HaveCount(12);
            actualRunden.Should().NotContain(r => r.CoiffeurTrumpf.Typ == CoiffeurTrumpfTyp.Gschobna);
            actualRunden.Should().NotContain(r => r.CoiffeurTrumpf.Typ == CoiffeurTrumpfTyp.Differenzler);
        }
    }
}