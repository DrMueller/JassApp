using FluentAssertions;
using JassApp.Domain.Coiffeur.Models;
using Xunit;

namespace JassApp.UnitTests.Domain.Coiffeur.Models
{
    public class CoiffeurSpielrundeOptionenUnitTests
    {
        [Theory]
        [InlineData(true, true, "Raucherpausen: Ja, Shots: Ja")]
        [InlineData(true, false, "Raucherpausen: Ja, Shots: Nein")]
        [InlineData(false, true, "Raucherpausen: Nein, Shots: Ja")]
        [InlineData(false, false, "Raucherpausen: Nein, Shots: Nein")]
        public void Description_IsFormatted(bool raucher, bool shots, string expected)
        {
            var sut = new CoiffeurSpielrundeOptionen(raucher, shots);

            sut.Description.Should().Be(expected);
        }
    }
}
