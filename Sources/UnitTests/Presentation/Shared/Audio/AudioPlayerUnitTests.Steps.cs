using JassApp.Presentation.Infrastructure.JavaScript.Services;
using JassApp.Presentation.Shared.Audio;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Moq;
using MudBlazor.Services;

namespace JassApp.UnitTests.Presentation.Shared.Audio
{
    public partial class AudioPlayerUnitTests
    {
        private static async Task<string> RenderAudioPlayerAsync(string audioSource, string voiceIcon)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddMudServices();
            services.AddSingleton(Mock.Of<IJavaScriptLocator>());
            services.AddSingleton(Mock.Of<IJSRuntime>());
            using var serviceProvider = services.BuildServiceProvider();
            await using var renderer = new HtmlRenderer(
                serviceProvider,
                serviceProvider.GetRequiredService<ILoggerFactory>());

            return await renderer.Dispatcher.InvokeAsync(async () =>
            {
                var component = await renderer.RenderComponentAsync<AudioPlayer>(
                    ParameterView.FromDictionary(new Dictionary<string, object?>
                    {
                        [nameof(AudioPlayer.Src)] = audioSource,
                        [nameof(AudioPlayer.VoiceIcon)] = voiceIcon
                    }));

                return component.ToHtmlString();
            });
        }
    }
}