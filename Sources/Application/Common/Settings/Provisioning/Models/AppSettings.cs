using JetBrains.Annotations;

namespace JassApp.Common.Settings.Provisioning.Models
{
    [PublicAPI]
    public class AppSettings
    {
        public const string SectionKey = "AppSettings";
        public string AppVersion { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
        public string GitHubCommit { get; set; } = null!;
    }
}