using JassApp.DataAccess.Tables;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Shared.Data.Querying;

namespace JassApp.Domain.Spieler.Specifications
{
    public record SpielerHistoryEntryBo(
        string Spieler1Name,
        string Spieler2Name,
        DateTime GespieltAm);

    public class SpielerHistorySpec : IQuerySpecification<SpielerHistoryEntryBo>
    {
        public IQueryable<SpielerHistoryEntryBo> Apply(IQueryBase qryProvider)
        {
            var spielerQry = from jassTeamSpieler in qryProvider.Query<JassTeamSpielerTable>()
                join team in qryProvider.Query<JassTeamTable>() on jassTeamSpieler.JassTeamId equals team.Id
                join spieler in qryProvider.Query<SpielerTable>() on jassTeamSpieler.SpielerId equals spieler.Id
                select new
                {
                    jassTeamSpieler.Position,
                    SpielerName = spieler.Name,
                    jassTeamSpieler.JassTeamId
                };

            var qry = from runde in qryProvider.Query<CoiffeurSpielrundeTable>()
                join jassTeam in qryProvider.Query<JassTeamTable>() on runde.Id equals jassTeam.CoiffeurSpielrundeId
                join spieler in spielerQry on jassTeam.Id equals spieler.JassTeamId into grpSpieler
                orderby runde.GestartetAm descending
                select new SpielerHistoryEntryBo(
                    grpSpieler.Single(f => f.Position == JassTeamSpielerPosition.Spieler1).SpielerName,
                    grpSpieler.Single(f => f.Position == JassTeamSpielerPosition.Spieler2).SpielerName,
                    runde.GestartetAm);

            return qry;
        }
    }
}