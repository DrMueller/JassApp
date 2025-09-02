namespace JassApp.Domain.Shared.Data.Querying
{
    public interface IQuerySpecification<out TResult>
    {
        IQueryable<TResult> Apply(IQueryBase qryProvider);
    }
}