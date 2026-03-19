using System.IdentityModel.Tokens.Jwt;
using JassApp.Common.Logging.Services;
using JassApp.Common.Logging.Services.Models;
using JetBrains.Annotations;

namespace JassApp.Presentation.Infrastructure.Logging.Services
{
    [UsedImplicitly]
    public class LogInfoProvider(IHttpContextAccessor httpContextAccessor) : ILogInfoProvider
    {
        private bool IsIdentityAuthenticated
        {
            get
            {
                var identityExists = httpContextAccessor.HttpContext?.User.Identity;
                return identityExists is { IsAuthenticated: true };
            }
        }

        public LogInfo ProvideLogInfo()
        {
            if (!IsIdentityAuthenticated)
            {
                return LogInfo.CreateAnonymous();
            }

            var identityName = httpContextAccessor.HttpContext?.User.Claims.SingleOrDefault(f => f.Type == JwtRegisteredClaimNames.Name)?.Value;
            if (string.IsNullOrEmpty(identityName))
            {
                return LogInfo.CreateAnonymous();
            }

            var logInfo = new LogInfo(identityName);
            return logInfo;
        }
    }
}