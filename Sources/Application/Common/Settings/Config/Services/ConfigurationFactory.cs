using System.Reflection;

namespace JassApp.Common.Settings.Config.Services
{
    public static class ConfigurationFactory
    {
        public static IConfiguration Create(Assembly sourceAssembly)
        {
            var path = Path.GetDirectoryName(sourceAssembly.Location);

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(path!)
                .AddJsonFile("appsettings.json", false, true);

            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            if (isDevelopment)
            {
                configBuilder.AddUserSecrets(typeof(ConfigurationFactory).Assembly);
            }

            return configBuilder.Build();
        }
    }
}