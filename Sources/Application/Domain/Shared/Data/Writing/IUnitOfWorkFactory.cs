namespace JassApp.Domain.Shared.Data.Writing
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}