using FluentAssertions;
using JassApp.Domain.Coiffeur.Models;
using Xunit;

namespace JassApp.UnitTests.Domain.Coiffeur.Models
{
    public class CoiffeurTrumpfUnitTests
    {
        [Fact]
        public void CreateAll_ReturnsAllTrumpfs_WithUniqueTypes()
        {
            var all = CoiffeurTrumpf.CreateAll();

            all.Select(t => t.Typ).Should().OnlyHaveUniqueItems();
            all.Should().Contain(t => t.Typ == CoiffeurTrumpfTyp.Herz);
            all.Should().Contain(t => t.Typ == CoiffeurTrumpfTyp.Wahl);
        }

        [Theory]
        [InlineData(CoiffeurTrumpfTyp.Herz, "Herz")]
        [InlineData(CoiffeurTrumpfTyp.Ecken, "Ecken")]
        [InlineData(CoiffeurTrumpfTyp.Kreuz, "Kreuz")]
        [InlineData(CoiffeurTrumpfTyp.Schaufeln, "Schaufeln")]
        [InlineData(CoiffeurTrumpfTyp.Gschobna, "Gschobna")]
        [InlineData(CoiffeurTrumpfTyp.Differenzler, "Differenzler")]
        [InlineData(CoiffeurTrumpfTyp.Oben, "Oben")]
        [InlineData(CoiffeurTrumpfTyp.Unten, "Unten")]
        [InlineData(CoiffeurTrumpfTyp.Slalom, "Slalom")]
        [InlineData(CoiffeurTrumpfTyp.Misere, "Misere")]
        [InlineData(CoiffeurTrumpfTyp.Gustav, "Gustav")]
        [InlineData(CoiffeurTrumpfTyp.DrueliDrue, "Drülidrü")]
        [InlineData(CoiffeurTrumpfTyp.Tutti, "Tutti")]
        [InlineData(CoiffeurTrumpfTyp.Wahl, "Wahl")]
        public void CreateFromTyp_ReturnsSingleton(CoiffeurTrumpfTyp typ, string expectedName)
        {
            var trumpf = CoiffeurTrumpf.CreateFromTyp(typ);

            trumpf.Typ.Should().Be(typ);
            trumpf.Name.Should().Be(expectedName);
        }
    }
}