using JassApp.DataAccess.DbContexts.Factories;
using JassApp.Domain.Shared.Data.Writing;
using JetBrains.Annotations;

namespace JassApp.DataAccess.UnitOfWorks.Implementation
{
    [UsedImplicitly]
    internal class UnitOfWorkFactory(
        Func<IUnitOfWork> unitOfWorkFactory,
        IAppDbContextFactory dbContextFactory)
        : IUnitOfWorkFactory
    {
        public IUnitOfWork Create()
        {
            var dbContext = dbContextFactory.Create();
            var unitOfWork = unitOfWorkFactory();
            ((UnitOfWork)unitOfWork).Initialize(dbContext);

            return unitOfWork;
        }
    }
}