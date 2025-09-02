using JassApp.Common.InformationHandling;
using JassApp.Domain.Shared.Data.Writing;

namespace JassApp.Domain.Spieler.Services
{
    public interface ISpielerRepository : IRepository
    {
        Task<InformationEntries> DeleteAsync(int spielerId);
        Task<InformationEntries> SaveAsync(Models.Spieler spieler);
    }
}