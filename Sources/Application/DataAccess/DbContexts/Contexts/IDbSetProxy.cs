namespace JassApp.DataAccess.DbContexts.Contexts
{
    public interface IDbSetProxy<TEntity>
        where TEntity : class
    {
        Task AddAsync(TEntity entity);

        IQueryable<TEntity> AsNoTracking();

        IQueryable<TEntity> AsQueryable();

        void Remove(TEntity entity);
    }
}