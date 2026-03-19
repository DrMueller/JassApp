using JassApp.Common.LanguageExtensions.Invariance;

namespace JassApp.Common.Logging.Services.Models
{
    public class LogInfo
    {
        private const string AnonymousEmail = "Anonymous";

        public string BenutzerEmail { get; }

        public LogInfo(string benutzerEmail)
        {
            Guard.StringNotNullOrEmpty(() => benutzerEmail);

            BenutzerEmail = benutzerEmail;
        }

        public static LogInfo CreateAnonymous()
        {
            return new LogInfo(AnonymousEmail);
        }
    }
}