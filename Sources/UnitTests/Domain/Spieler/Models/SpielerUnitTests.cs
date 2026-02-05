using FluentAssertions;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Spieler.Models;
using Xunit;

namespace JassApp.UnitTests.Domain.Spieler.Models
{
    public class SpielerUnitTests
    {
        [Fact]
        public void CanBeDeleted_IsTrue_WhenNoAssignedTeams()
        {
            var sut = new global::JassApp.Domain.Spieler.Models.Spieler(new SpielerId(1), "Test", Array.Empty<JassTeamId>());

            sut.CanBeDeleted.Should().BeTrue();
        }

        [Fact]
        public void CanBeDeleted_IsFalse_WhenAssignedTeamsExist()
        {
            var sut = new global::JassApp.Domain.Spieler.Models.Spieler(new SpielerId(1), "Test", new[] { new JassTeamId(1) });

            sut.CanBeDeleted.Should().BeFalse();
        }

        [Fact]
        public void UpdateName_ChangesName()
        {
            var sut = new global::JassApp.Domain.Spieler.Models.Spieler(new SpielerId(1), "Old", Array.Empty<JassTeamId>());

            sut.UpdateName("New");

            sut.Name.Should().Be("New");
        }
    }
}
