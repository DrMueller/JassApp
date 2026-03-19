using JassApp.Common.Logging.Services.Servants.Implementation;
using JassApp.DataAccess.Querying;
using JassApp.DataAccess.UnitOfWorks.Implementation;
using JassApp.Domain.Shared.Data.Querying;
using JassApp.Domain.Shared.Data.Writing;
using JetBrains.Annotations;
using Lamar;
using Microsoft.ApplicationInsights.Extensibility;

namespace JassApp
{
    [UsedImplicitly]
    public class ServiceRegistryCollection : ServiceRegistry
    {
        public ServiceRegistryCollection()
        {
            Scan(scanner =>
            {
                scanner.AssemblyContainingType<ServiceRegistryCollection>();
                scanner.AddAllTypesOf<IRepository>();
                scanner.WithDefaultConventions();
            });

            For<ITelemetryInitializer>().Use<CloudRoleInstanceTelemetryInitializer>().Singleton();
            For<ITelemetryInitializer>().Use<AuthenticatedUserIdTelemetryInitializer>().Scoped();
            For<IQueryService>().Use<QueryService>().Singleton();
            For<IUnitOfWorkFactory>().Use<UnitOfWorkFactory>().Singleton();
        }
    }
}