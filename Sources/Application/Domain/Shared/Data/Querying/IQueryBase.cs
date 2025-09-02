using JassApp.DataAccess.Tables.Base;

namespace JassApp.Domain.Shared.Data.Querying
{
    public interface IQueryBase
    {
        IQueryable<T> Query<T>()
            where T : TableBase;
    }
}