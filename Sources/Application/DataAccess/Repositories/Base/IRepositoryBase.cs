using JassApp.DataAccess.DbContexts.Contexts;

namespace JassApp.DataAccess.Repositories.Base
{
    public interface IRepositoryBase
    {
        internal void Initialize(IAppDbContext dbContext);
    }
}