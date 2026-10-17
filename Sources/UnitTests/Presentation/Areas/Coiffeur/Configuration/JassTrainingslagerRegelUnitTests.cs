using FluentAssertions;
using JassApp.Presentation.Areas.Coiffeur.Configuration;
using Xunit;

namespace JassApp.UnitTests.Presentation.Areas.Coiffeur.Configuration
{
    public partial class JassTrainingslagerRegelUnitTests
    {
        [Fact]
        public void SelectingAllJassTrainingslagerSpieler_RegardlessOfOrder_ReturnsTrue()
        {
            // Arrange
            var (founi, leiser, mueller, linse) = CreateJassTrainingslagerSpieler();
            var selectedSpieler = new[] { linse, founi, mueller, leiser };

            // Act
            var shouldSelect = JassTrainingslagerRegel.ShouldAutoSelect(selectedSpieler);

            // Assert
            shouldSelect.Should().BeTrue();
        }

        [Fact]
        public void SelectingDifferentJassTrainingslagerSpieler_ReturnsFalse()
        {
            // Arrange
            var (founi, leiser, mueller, _) = CreateJassTrainingslagerSpieler();
            var selectedSpieler = new[] { founi, leiser, mueller, CreateOtherSpieler() };

            // Act
            var shouldSelect = JassTrainingslagerRegel.ShouldAutoSelect(selectedSpieler);

            // Assert
            shouldSelect.Should().BeFalse();
        }

        [Fact]
        public void SelectingIncompleteJassTrainingslagerSpieler_ReturnsFalse()
        {
            // Arrange
            var (founi, leiser, mueller, _) = CreateJassTrainingslagerSpieler();
            var selectedSpieler = new[] { founi, leiser, mueller, null };

            // Act
            var shouldSelect = JassTrainingslagerRegel.ShouldAutoSelect(selectedSpieler);

            // Assert
            shouldSelect.Should().BeFalse();
        }
    }
}