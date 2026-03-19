using JassApp.Common.Logging.Services.Models;

namespace JassApp.Common.Logging.Services
{
    public interface ILogInfoProvider
    {
        LogInfo ProvideLogInfo();
    }
}