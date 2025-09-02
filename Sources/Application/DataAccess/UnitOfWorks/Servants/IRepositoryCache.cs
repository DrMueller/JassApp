using JassApp.DataAccess.DbContexts.Contexts;
using JassApp.Domain.Shared.Data.Writing;

namespace JassApp.DataAccess.UnitOfWorks.Servants
{
    public interface IRepositoryCache
    {
        TRepo GetRepository<TRepo>(IAppDbContext dbContext)
            where TRepo : IRepository;
    }
}