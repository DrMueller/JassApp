using JassApp.Common;
using JassApp.Common.Settings.Provisioning.Services;

namespace JassApp.Presentation.Infrastructure.Caching.Controllers.Implementation
{
    public class CachingService(ISettingsProvider settingsProvider) : ICachingService
    {
        private const string SuffixTemplate = "?v={0}";

        private static Lazy<string> LocalTicks { get; } = new(DateTime.Now.Ticks.ToString());

        public Task<string> LoadCachingSuffixAsync()
        {
            var version = GetAppVersion();
            var result = string.Format(SuffixTemplate, version);

            return Task.FromResult(result);
        }

        private string GetAppVersion()
        {
            var appSettingVersion = settingsProvider.AppSettings.AppVersion;

            return appSettingVersion == Constants.SemVerVariable ? LocalTicks.Value : appSettingVersion;
        }
    }
}