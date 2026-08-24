using FluentAssertions;
using JassApp.Domain.Coiffeur.Models;
using Xunit;

namespace JassApp.UnitTests.Domain.Coiffeur.Models
{
    public class CoiffeurSpielrundeOptionenUnitTests
    {
        [Theory]
        [InlineData(true, true, false, "Raucherpausen: Ja, Shots: Ja, JTL: Nein")]
        [InlineData(true, false, false, "Raucherpausen: Ja, Shots: Nein, JTL: Nein")]
        [InlineData(false, true, false, "Raucherpausen: Nein, Shots: Ja, JTL: Nein")]
        [InlineData(false, false, false, "Raucherpausen: Nein, Shots: Nein, JTL: Nein")]
        [InlineData(false, false, true, "Raucherpausen: Nein, Shots: Nein, JTL: Ja")]
        public void Description_IsFormatted(bool raucher, bool shots, bool isJasstrainingslager, string expected)
        {
            var sut = new CoiffeurSpielrundeOptionen(raucher, shots, isJasstrainingslager);

            sut.Description.Should().Be(expected);
        }
    }
}