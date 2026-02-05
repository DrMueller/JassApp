using FluentAssertions;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Spieler.Models;
using Xunit;

namespace JassApp.UnitTests.Domain.Coiffeur.Models
{
    public class JassTeamUnitTests
    {
        [Fact]
        public void Description_ReturnsSpielerNames()
        {
            var s1 = new JassTeamSpieler(new JassTeamSpielerId(1), new SpielerId(1), "A", false, JassTeamSpielerPosition.Spieler1);
            var s2 = new JassTeamSpieler(new JassTeamSpielerId(2), new SpielerId(2), "B", false, JassTeamSpielerPosition.Spieler2);

            var sut = new JassTeam(new JassTeamId(1), JassTeamTyp.Team1, new[] { s1, s2 });

            sut.Description.Should().Be("A & B");
        }

        [Fact]
        public void GetRundeDescription_MarksStartPlayerAndActivePlayer()
        {
            var s1 = new JassTeamSpieler(new JassTeamSpielerId(1), new SpielerId(1), "A", true, JassTeamSpielerPosition.Spieler1);
            var s2 = new JassTeamSpieler(new JassTeamSpielerId(2), new SpielerId(2), "B", false, JassTeamSpielerPosition.Spieler2);

            var sut = new JassTeam(new JassTeamId(1), JassTeamTyp.Team1, new[] { s1, s2 });

            sut.GetRundeDescription(s1).Should().Be("[A*] & B");
        }
    }
}
