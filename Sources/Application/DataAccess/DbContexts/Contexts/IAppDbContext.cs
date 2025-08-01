using System.Diagnostics.CodeAnalysis;
using JassApp.DataAccess.Tables.Base;

namespace JassApp.DataAccess.DbContexts.Contexts
{
    public interface IAppDbContext : IDisposable
    {
        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Same name as the one on the DbContext needed")]
        IDbSetProxy<TTable> DbSet<TTable>() where TTable : TableBase;

        Task<int> SaveChangesAsync(CancellationToken token = default);
    }
}