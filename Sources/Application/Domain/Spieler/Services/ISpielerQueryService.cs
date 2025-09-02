using JassApp.Domain.Spieler.BusinessObjects;

namespace JassApp.Domain.Spieler.Services
{
    public interface ISpielerQueryService
    {
        Task<IReadOnlyCollection<SpielerHistoryEntryBo>> LoadHistoryAsync();
    }
}