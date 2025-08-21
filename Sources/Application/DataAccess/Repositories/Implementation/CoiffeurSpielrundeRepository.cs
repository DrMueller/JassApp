using JassApp.Common.InformationHandling;
using JassApp.Common.LanguageExtensions.Types.Eithers;
using JassApp.DataAccess.DbContexts.Contexts;
using JassApp.DataAccess.DbContexts.Factories;
using JassApp.DataAccess.Tables;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Coiffeur.Repositories;
using JassApp.Domain.Spieler.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace JassApp.DataAccess.Repositories.Implementation
{
    [UsedImplicitly]
    public class CoiffeurSpielrundeRepository(IAppDbContextFactory appDbContextFactory) : ICoiffeurSpielrundeRepository
    {
        public async Task DeleteAsync(CoiffeurSpielrundeId rundeId)
        {
            using var context = appDbContextFactory.Create();
            var dbSet = context.DbSet<CoiffeurSpielrundeTable>();

            var rundeTable = await LoadSertAsync(context, rundeId);
            dbSet.Remove(rundeTable);
            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<CoiffeurSpielrunde>> LoadAllAsync()
        {
            using var context = appDbContextFactory.Create();
            var data = await QueryCoiffeurSpielrunde(context).ToListAsync();

            return data.Select(Map).ToList();
        }

        public async Task<CoiffeurSpielrunde> LoadAsync(CoiffeurSpielrundeId rundeId)
        {
            using var context = appDbContextFactory.Create();
            var data = await QueryCoiffeurSpielrunde(context).SingleAsync(f => f.Id == rundeId.Value);

            return Map(data);
        }

        public async Task<Either<InformationEntries, int>> SaveAsync(CoiffeurSpielrunde runde)
        {
            using var context = appDbContextFactory.Create();
            var rundeTable = await LoadSertAsync(context, runde.Id);

            rundeTable.Punktewert = runde.PunkteWert;
            rundeTable.GestartetAm = runde.GestartetAm;

            var jassTeamTable1 = rundeTable.JassTeams.SingleOrDefault(f => f.JassTeamTyp == JassTeamTyp.Team1);

            if (jassTeamTable1 == null)
            {
                jassTeamTable1 = new JassTeamTable();
                rundeTable.JassTeams.Add(jassTeamTable1);
            }

            jassTeamTable1.JassTeamTyp = JassTeamTyp.Team1;
            jassTeamTable1.JassTeamSpieler1.SpielerId = runde.Team1.Spieler1.SpielerId.Value;
            jassTeamTable1.JassTeamSpieler1.IstStartSpieler = runde.Team1.Spieler1.IstStartSpieler;
            jassTeamTable1.JassTeamSpieler2.SpielerId = runde.Team1.Spieler2.SpielerId.Value;
            jassTeamTable1.JassTeamSpieler2.IstStartSpieler = runde.Team1.Spieler2.IstStartSpieler;

            var jassTeamTable2 = rundeTable.JassTeams.SingleOrDefault(f => f.JassTeamTyp == JassTeamTyp.Team2);

            if (jassTeamTable2 == null)
            {
                jassTeamTable2 = new JassTeamTable();
                rundeTable.JassTeams.Add(jassTeamTable2);
            }

            jassTeamTable2.JassTeamTyp = JassTeamTyp.Team2;
            jassTeamTable2.JassTeamSpieler1.SpielerId = runde.Team2.Spieler1.SpielerId.Value;
            jassTeamTable2.JassTeamSpieler1.IstStartSpieler = runde.Team2.Spieler1.IstStartSpieler;
            jassTeamTable2.JassTeamSpieler2.SpielerId = runde.Team2.Spieler2.SpielerId.Value;
            jassTeamTable2.JassTeamSpieler2.IstStartSpieler = runde.Team2.Spieler2.IstStartSpieler;

            MapTrumpfrunden(runde, rundeTable);
            await context.SaveChangesAsync();

            return rundeTable.Id;
        }

        private static async Task<CoiffeurSpielrundeTable> LoadSertAsync(
            IAppDbContext context,
            CoiffeurSpielrundeId id)
        {
            if (id.Value > 0)
            {
                return await QueryCoiffeurSpielrunde(context).SingleAsync(f => f.Id == id.Value);
            }

            var runde = new CoiffeurSpielrundeTable
            {
                JassTeams = new List<JassTeamTable>(),
                Trumpfrunden = new List<TrumpfrundeTable>()
            };

            await context.DbSet<CoiffeurSpielrundeTable>().AddAsync(runde);

            return runde;
        }

        private static TrumpfrundeTable LoadSertTrumpfrundeTable(CoiffeurSpielrundeTable rundeTable, CoiffeurTrumpfrunde coiffeurTrumpfrunde)
        {
            if (coiffeurTrumpfrunde.ID.Value == 0)
            {
                var trumpfrundeTable = new TrumpfrundeTable();
                rundeTable.Trumpfrunden.Add(trumpfrundeTable);

                return trumpfrundeTable;
            }

            return rundeTable.Trumpfrunden.Single(f => f.Id == coiffeurTrumpfrunde.ID.Value);
        }

        private static CoiffeurSpielrunde Map(CoiffeurSpielrundeTable table)
        {
            var trumpfrunden = table
                .Trumpfrunden
                .Select(f => new CoiffeurTrumpfrunde(
                    new TrumpfrundeId(f.Id),
                    f.PunkteModifikator,
                    CoiffeurTrumpf.CreateFromTyp(f.CoiffeurTrumpfTyp),
                    f.ResultatTeam1,
                    f.ResultatTeam2))
                .ToList();

            var team1 = table.JassTeams.Single(f => f.JassTeamTyp == JassTeamTyp.Team1);
            var team2 = table.JassTeams.Single(f => f.JassTeamTyp == JassTeamTyp.Team2);

            var jassTeam1 = new JassTeam(new JassTeamÎd(team1.Id),
                new JassTeamSpieler(
                    new JassTeamSpielerId(team1.JassTeamSpieler1Id),
                    new SpielerId(team1.JassTeamSpieler1.SpielerId),
                    team1.JassTeamSpieler1.Spieler.Name,
                    team1.JassTeamSpieler1.IstStartSpieler),
                new JassTeamSpieler(
                    new JassTeamSpielerId(team1.JassTeamSpieler2Id),
                    new SpielerId(team1.JassTeamSpieler2.SpielerId),
                    team1.JassTeamSpieler2.Spieler.Name,
                    team1.JassTeamSpieler2.IstStartSpieler));

            var jassTeam2 = new JassTeam(new JassTeamÎd(team2.Id),
                new JassTeamSpieler(
                    new JassTeamSpielerId(team2.JassTeamSpieler1Id),
                    new SpielerId(team2.JassTeamSpieler1.SpielerId),
                    team2.JassTeamSpieler1.Spieler.Name,
                    team2.JassTeamSpieler1.IstStartSpieler),
                new JassTeamSpieler(new JassTeamSpielerId(team2.JassTeamSpieler2Id),
                    new SpielerId(team2.JassTeamSpieler2.SpielerId),
                    team2.JassTeamSpieler2.Spieler.Name,
                    team2.JassTeamSpieler2.IstStartSpieler));

            return new CoiffeurSpielrunde(
                new CoiffeurSpielrundeId(table.Id),
                table.GestartetAm,
                table.Punktewert,
                trumpfrunden,
                jassTeam1,
                jassTeam2);
        }

        private static void MapTrumpfrunden(CoiffeurSpielrunde runde, CoiffeurSpielrundeTable rundeTable)
        {
            foreach (var trumpfrunde in runde.Trumpfrunden)
            {
                var trumpfrundeTable = LoadSertTrumpfrundeTable(rundeTable, trumpfrunde);
                trumpfrundeTable.PunkteModifikator = trumpfrunde.PunkteModifikator;
                trumpfrundeTable.CoiffeurTrumpfTyp = trumpfrunde.CoiffeurTrumpf.Typ;
                trumpfrundeTable.ResultatTeam1 = trumpfrunde[JassTeamTyp.Team1].RawInput;
                trumpfrundeTable.ResultatTeam2 = trumpfrunde[JassTeamTyp.Team2].RawInput;
            }
        }

        private static IQueryable<CoiffeurSpielrundeTable> QueryCoiffeurSpielrunde(IAppDbContext context)
        {
            return context.DbSet<CoiffeurSpielrundeTable>().AsQueryable()
                .Include(f => f.JassTeams)
                .ThenInclude(f => f.JassTeamSpieler1)
                .ThenInclude(f => f.Spieler)
                .Include(f => f.JassTeams)
                .ThenInclude(f => f.JassTeamSpieler2)
                .ThenInclude(f => f.Spieler)
                .Include(f => f.Trumpfrunden);
        }
    }
}