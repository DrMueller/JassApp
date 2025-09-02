using JassApp.DataAccess.Querying;
using JassApp.DataAccess.UnitOfWorks.Implementation;
using JassApp.Domain.Shared.Data.Querying;
using JassApp.Domain.Shared.Data.Writing;
using JetBrains.Annotations;
using Lamar;

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

            For<IQueryService>().Use<QueryService>().Singleton();
            For<IUnitOfWorkFactory>().Use<UnitOfWorkFactory>().Singleton();
        }
    }
}