using JetBrains.Annotations;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace JassApp.Common.Logging.Services.Servants.Implementation
{
    [UsedImplicitly]
    internal class AuthenticatedUserIdTelemetryInitializer(ILogInfoProvider logInfoProvider) : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            var logInfo = logInfoProvider.ProvideLogInfo();
            telemetry.Context.User.AuthenticatedUserId = logInfo.BenutzerEmail;
        }
    }
}