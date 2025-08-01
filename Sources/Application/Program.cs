using JassApp.Common.Settings.Config.Services;
using JassApp.Presentation.Shell.Initialization;
using JetBrains.Annotations;
using Lamar.Microsoft.DependencyInjection;

namespace JassApp
{
    [UsedImplicitly]
    public class Program
    {
        public static IConfiguration Configuration { get; } = ConfigurationFactory.Create(typeof(Program).Assembly);

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseLamar(serviceRegistry =>
            {
                serviceRegistry.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory();
                    scanner.LookForRegistries();
                });
            });

            ServiceInitialization.Initialize(builder.Services);
            var app = builder.Build();
            AppInitialization.Initialize(app);

            app.Run();
        }
    }
}