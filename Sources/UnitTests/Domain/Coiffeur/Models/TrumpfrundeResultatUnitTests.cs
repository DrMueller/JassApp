using FluentAssertions;
using JassApp.Domain.Coiffeur.Models;
using Xunit;

namespace JassApp.UnitTests.Domain.Coiffeur.Models
{
    public class TrumpfrundeResultatUnitTests
    {
        [Fact]
        public void DefaultCtor_StartsUnplayed()
        {
            var sut = new TrumpfrundeResultat(JassTeamTyp.Team1);

            sut.IstGespielt.Should().BeFalse();
            sut.Punkte.Should().BeNull();
            sut.IstMatch.Should().BeFalse();
            sut.IstKonterMatch.Should().BeFalse();
        }

        [Fact]
        public void IstGespielt_IsTrue_WhenPunkteSet()
        {
            var sut = new TrumpfrundeResultat(JassTeamTyp.Team1);
            sut.Punkte = 10;

            sut.IstGespielt.Should().BeTrue();
        }
    }
}
