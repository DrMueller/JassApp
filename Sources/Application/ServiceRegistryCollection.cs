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
                scanner.WithDefaultConventions();
            });
        }
    }
}