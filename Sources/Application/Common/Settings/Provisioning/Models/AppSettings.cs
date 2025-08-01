using JetBrains.Annotations;

namespace JassApp.Common.Settings.Provisioning.Models
{
    [PublicAPI]
    public class AppSettings
    {
        public const string SectionKey = "AppSettings";
        public string ConnectionString { get; set; } = null!;
    }
}