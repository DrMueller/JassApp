using JassApp.Common.InformationHandling;

namespace JassApp.Domain.Spieler.Services
{
    public interface ISpielerRepository
    {
        Task<InformationEntries> SaveAsync(Models.Spieler spieler);
        Task<InformationEntries> DeleteAsync(int spielerId);
        Task<IReadOnlyCollection<Models.Spieler>> LoadAllAsync();
        Task<Models.Spieler> LoadAsync(int spielerId);
    }
}
