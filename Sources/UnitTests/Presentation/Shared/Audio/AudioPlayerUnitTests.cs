using FluentAssertions;
using Xunit;

namespace JassApp.UnitTests.Presentation.Shared.Audio
{
    public partial class AudioPlayerUnitTests
    {
        [Fact]
        public async Task RenderingWithVoiceIcon_UsesSuppliedIcon()
        {
            // Arrange
            const string audioSource = "/audio/test.mp3";
            const string suppliedIcon = "<svg><title>LongPauseIndicator</title></svg>";

            // Act
            var renderedHtml = await RenderAudioPlayerAsync(audioSource, suppliedIcon);

            // Assert
            renderedHtml.Should().Contain(suppliedIcon);
        }
    }
}