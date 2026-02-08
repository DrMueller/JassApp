using JassApp.Common.InformationHandling;
using JassApp.Common.LanguageExtensions.Types.Eithers;
using JassApp.DataAccess.Extensions;
using JassApp.DataAccess.Repositories.Base;
using JassApp.DataAccess.Tables;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Coiffeur.Repositories;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace JassApp.DataAccess.Repositories
{
    [UsedImplicitly]
    public class CoiffeurSpielrundeRepository : RepositoryBase, ICoiffeurSpielrundeRepository
    {
        public async Task DeleteAsync(CoiffeurSpielrundeId rundeId)
        {
            var rundeTable = await LoadSertAsync(rundeId);
            Remove(rundeTable);
        }

        public async Task<Either<InformationEntries, CoiffeurSpielrundeTable>> SaveAsync(CoiffeurSpielrunde runde)
        {
            var rundeTable = await LoadSertAsync(runde.Id);

            rundeTable.Punktewert = runde.PunkteWert;
            rundeTable.GestartetAm = runde.GestartetAm;
            rundeTable.DoIncludeRaucherpausen = runde.Optionen.DoIncludeRaucherpausen;
            rundeTable.DoIncludeShots = runde.Optionen.DoIncludeShots;

            MapJassTeams(runde, rundeTable);
            MapTrumpfrunden(runde, rundeTable);

            return rundeTable;
        }

        private static void MapJassTeam(JassTeam model, JassTeamTable table)
        {
            var spieler1Table = table.JassTeamSpieler.SingleOrAdd(f => f.Position == JassTeamSpielerPosition.Spieler1);
            var spieler2Table = table.JassTeamSpieler.SingleOrAdd(f => f.Position == JassTeamSpielerPosition.Spieler2);

            MapSieler(model.Spieler1, spieler1Table, JassTeamSpielerPosition.Spieler1);
            MapSieler(model.Spieler2, spieler2Table, JassTeamSpielerPosition.Spieler2);
        }

        private static void MapSieler(JassTeamSpieler model, JassTeamSpielerTable table, JassTeamSpielerPosition position)
        {
            table.Position = position;
            table.IstStartSpieler = model.IstStartSpieler;
            table.SpielerId = model.SpielerId.Value;
            table.Position = model.Position;
        }

        private static void MapTrumpfrunden(CoiffeurSpielrunde runde, CoiffeurSpielrundeTable spielrundeTable)
        {
            foreach (var trumpfrunde in runde.Trumpfrunden)
            {
                var trumpfrundeTable = spielrundeTable.Trumpfrunden.SingleOrAddById(trumpfrunde.ID.Value);
                trumpfrundeTable.PunkteModifikator = trumpfrunde.PunkteModifikator;
                trumpfrundeTable.CoiffeurTrumpfTyp = trumpfrunde.CoiffeurTrumpf.Typ;
                trumpfrundeTable.ResultatTeam1 = trumpfrunde[JassTeamTyp.Team1].Punkte;
                trumpfrundeTable.IstMatschTeam1 = trumpfrunde[JassTeamTyp.Team1].IstMatch;
                trumpfrundeTable.IstKonterMatchTeam1 = trumpfrunde[JassTeamTyp.Team1].IstKontermatsch;

                trumpfrundeTable.ResultatTeam2 = trumpfrunde[JassTeamTyp.Team2].Punkte;
                trumpfrundeTable.IstMatschTeam2 = trumpfrunde[JassTeamTyp.Team2].IstMatch;
                trumpfrundeTable.IstKonterMatchTeam2 = trumpfrunde[JassTeamTyp.Team2].IstKontermatsch;
            }
        }

        private async Task<CoiffeurSpielrundeTable> LoadSertAsync(
            CoiffeurSpielrundeId id)
        {
            if (id.Value > 0)
            {
                return await QueryCoiffeurSpielrunde().SingleAsync(f => f.Id == id.Value);
            }

            var runde = new CoiffeurSpielrundeTable();
            await AddAsync(runde);

            return runde;
        }

        private static void MapJassTeams(CoiffeurSpielrunde runde, CoiffeurSpielrundeTable rundeTable)
        {
            foreach (var jassTeam in runde.JassTeams)
            {
                var jassTeamTable = rundeTable.JassTeams.SingleOrAdd(f => f.JassTeamTyp == jassTeam.Typ);
                jassTeamTable.JassTeamTyp = jassTeam.Typ;
                MapJassTeam(jassTeam, jassTeamTable);
            }
        }

        private IQueryable<CoiffeurSpielrundeTable> QueryCoiffeurSpielrunde()
        {
            return Query<CoiffeurSpielrundeTable>()
                .Include(f => f.JassTeams)
                .ThenInclude(f => f.JassTeamSpieler)
                .ThenInclude(f => f.Spieler)
                .Include(f => f.Trumpfrunden);
        }
    }
}