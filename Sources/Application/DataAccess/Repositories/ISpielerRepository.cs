using JassApp.Common.InformationHandling;
using JassApp.Domain.Models;

namespace JassApp.DataAccess.Repositories
{
    public interface ISpielerRepository
    {
        Task<InformationEntries> SaveAsync(Spieler spieler);
        Task<InformationEntries> DeleteAsync(int spielerId);
        Task<IReadOnlyCollection<Spieler>> LoadAllAsync();
        Task<Spieler> LoadAsync(int spielerId);
    }
}
