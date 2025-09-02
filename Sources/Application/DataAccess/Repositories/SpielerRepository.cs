using JassApp.Common.InformationHandling;
using JassApp.DataAccess.Repositories.Base;
using JassApp.DataAccess.Tables;
using JassApp.Domain.Spieler.Models;
using JassApp.Domain.Spieler.Services;
using JetBrains.Annotations;

namespace JassApp.DataAccess.Repositories
{
    [UsedImplicitly]
    public class SpielerRepository : RepositoryBase, ISpielerRepository
    {
        public async Task<InformationEntries> DeleteAsync(int spielerId)
        {
            var spielerTable = await LoadSertAsync(spielerId);
            Remove(spielerTable);

            return InformationEntries.Empty;
        }

        public async Task<InformationEntries> SaveAsync(Spieler spieler)
        {
            var spielerTable = await LoadSertAsync(spieler.Id.Value);
            spielerTable.Name = spieler.Name;

            return InformationEntries.Empty;
        }

        private async Task<SpielerTable> LoadSertAsync(
            int id)
        {
            if (id == 0)
            {
                var newSpieler = new SpielerTable();
                await AddAsync(newSpieler);

                return newSpieler;
            }

            return await QuerySingleAsync<SpielerTable>(f => f.Id == id);
        }
    }
}