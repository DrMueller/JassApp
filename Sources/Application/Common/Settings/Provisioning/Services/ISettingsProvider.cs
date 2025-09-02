using JassApp.Common.Settings.Provisioning.Models;

namespace JassApp.Common.Settings.Provisioning.Services
{
    public interface ISettingsProvider
    {
        AppSettings AppSettings { get; }
    }
}