using JassApp.Common.Settings.Provisioning.Services;
using JetBrains.Annotations;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace JassApp.Common.Logging.Services.Servants.Implementation
{
    [UsedImplicitly]
    internal class CloudRoleInstanceTelemetryInitializer(ISettingsProvider settingsProvider) : ITelemetryInitializer
    {
        private const string RoleInstanceName = "JassApp";

        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleInstance = $"{RoleInstanceName}_{settingsProvider.AppSettings.EnvironmentName}";
        }
    }
}