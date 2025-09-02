namespace JassApp.Domain.Shared.Data.Querying
{
    public interface IQueryService
    {
        Task<bool> AnyAsync<TResult>(IQuerySpecification<TResult> spec);
        Task<IReadOnlyCollection<TResult>> QueryAsync<TResult>(IQuerySpecification<TResult> spec);
        Task<TResult> QuerySingleAsync<TResult>(IQuerySpecification<TResult> spec);
        Task<TResult?> QuerySingleOrDefaultAsync<TResult>(IQuerySpecification<TResult> spec);
    }
}