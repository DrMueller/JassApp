using JassApp.DataAccess.DbContexts.Contexts;

namespace JassApp.DataAccess.DbContexts.Factories
{
    public interface IAppDbContextFactory
    {
        IAppDbContext Create();
    }
}