using Microsoft.EntityFrameworkCore;

namespace JassApp.DataAccess.DbContexts.Factories
{
    public interface IDbContextOptionsFactory
    {
        DbContextOptions CreateForSqlServer(string connectionString);
    }
}