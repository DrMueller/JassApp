using JassApp.Common.InformationHandling;
using JassApp.DataAccess.DbContexts.Contexts;
using JassApp.DataAccess.DbContexts.Factories;
using JassApp.DataAccess.Tables;
using JassApp.Domain.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace JassApp.DataAccess.Repositories.Implementation
{
    [UsedImplicitly]
    public class SpielerRepository(IAppDbContextFactory appDbContextFactory) : ISpielerRepository
    {
        public async Task<InformationEntries> DeleteAsync(int spielerId)
        {
            using var appDbContext = appDbContextFactory.Create();
            var dbSet = appDbContext.DbSet<SpielerTable>();

            var spielerTable = await LoadSertAsync(spielerId, dbSet);
            dbSet.Remove(spielerTable);
            await appDbContext.SaveChangesAsync();

            return InformationEntries.Empty;
        }

        public async Task<IReadOnlyCollection<Spieler>> LoadAllAsync()
        {
            return await appDbContextFactory
                .Create()
                .DbSet<SpielerTable>()
                .AsQueryable()
                .Select(sp => new Spieler(new SpielerId(sp.Id), sp.Name))
                .ToListAsync();
        }

        public async Task<Spieler> LoadAsync(int spielerId)
        {
            var spielerTable = await appDbContextFactory
                .Create()
                .DbSet<SpielerTable>()
                .AsQueryable()
                .SingleAsync(f => f.Id == spielerId);

            return new Spieler(new SpielerId(spielerTable.Id), spielerTable.Name);
        }

        public async Task<InformationEntries> SaveAsync(Spieler spieler)
        {
            using var appDbContext = appDbContextFactory.Create();
            var dbSet = appDbContext.DbSet<SpielerTable>();

            var spielerTable = await LoadSertAsync(spieler.Id.Value, dbSet);
            spielerTable.Name = spieler.Name;
            await appDbContext.SaveChangesAsync();

            return InformationEntries.Empty;
        }

        private static async Task<SpielerTable> LoadSertAsync(
            int id,
            IDbSetProxy<SpielerTable> set)
        {
            if (id == 0)
            {
                var newSpieler = new SpielerTable();
                await set.AddAsync(newSpieler);

                return newSpieler;
            }

            return await set.AsQueryable().SingleAsync(f => f.Id == id);
        }
    }
}