using JassApp.DataAccess.DbContexts.Factories;
using JassApp.DataAccess.Tables;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Spieler.BusinessObjects;
using JassApp.Domain.Spieler.Services;
using Microsoft.EntityFrameworkCore;

namespace JassApp.DataAccess.QueryServices
{
    public class SpielerQueryService(IAppDbContextFactory appDbContextFactory) : ISpielerQueryService
    {
        public async Task<IReadOnlyCollection<SpielerHistoryEntryBo>> LoadHistoryAsync()
        {
            using var context = appDbContextFactory.Create();

            var spielerQry = from jassTeamSpieler in context.Query<JassTeamSpielerTable>()
                join team in context.Query<JassTeamTable>() on jassTeamSpieler.JassTeamId equals team.Id
                join spieler in context.Query<SpielerTable>() on jassTeamSpieler.SpielerId equals spieler.Id
                select new
                {
                    jassTeamSpieler.Position,
                    SpielerName = spieler.Name, jassTeamSpieler.JassTeamId
                };

            var qry = from runde in context.Query<CoiffeurSpielrundeTable>()
                join jassTeam in context.Query<JassTeamTable>() on runde.Id equals jassTeam.CoiffeurSpielrundeId
                join spieler in spielerQry on jassTeam.Id equals spieler.JassTeamId into grpSpieler
                orderby runde.GestartetAm descending
                select new SpielerHistoryEntryBo(
                    grpSpieler.Single(f => f.Position == JassTeamSpielerPosition.Spieler1).SpielerName,
                    grpSpieler.Single(f => f.Position == JassTeamSpielerPosition.Spieler2).SpielerName,
                    runde.GestartetAm);

            return await qry.ToListAsync();
        }
    }
}