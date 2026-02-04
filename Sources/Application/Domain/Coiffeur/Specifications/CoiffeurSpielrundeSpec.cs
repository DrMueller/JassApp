using JassApp.Common.LanguageExtensions.Types.Maybes;
using JassApp.Common.LanguageExtensions.Types.Maybes.Implementation;
using JassApp.DataAccess;
using JassApp.DataAccess.Tables;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Shared.Data.Querying;
using JassApp.Domain.Spieler.Models;

namespace JassApp.Domain.Coiffeur.Specifications
{
    public class CoiffeurSpielrundeSpec : IQuerySpecification<CoiffeurSpielrunde>
    {
        private readonly Maybe<CoiffeurSpielrundeId> _idMaybe = None.Value;

        public CoiffeurSpielrundeSpec()
        {
        }

        public CoiffeurSpielrundeSpec(CoiffeurSpielrundeId id)
        {
            _idMaybe = id;
        }

        public IQueryable<CoiffeurSpielrunde> Apply(IQueryBase qryProvider)
        {
            var qry = qryProvider.Query<CoiffeurSpielrundeTable>();
            qry = qry.WhereOptional(
                _idMaybe,
                f => q => q.Id == f.Value);

            var map = qry.Select(f =>
                new CoiffeurSpielrunde(
                    new CoiffeurSpielrundeId(f.Id),
                    f.GestartetAm,
                    f.Punktewert,
                    f.Trumpfrunden.Select(tr => new CoiffeurTrumpfrunde(
                        new TrumpfrundeId(tr.Id),
                        tr.PunkteModifikator,
                        CoiffeurTrumpf.CreateFromTyp(tr.CoiffeurTrumpfTyp),
                        new TrumpfrundeResultat(
                            JassTeamTyp.Team1,
                            tr.ResultatTeam1,
                            tr.IstMatschTeam1,
                            tr.IstKonterMatchTeam1),
                        new TrumpfrundeResultat(
                            JassTeamTyp.Team2,
                            tr.ResultatTeam2,
                            tr.IstMatschTeam2,
                            tr.IstKonterMatchTeam2))).ToList(),
                    f.JassTeams.Select(t => new JassTeam(
                        new JassTeamId(t.Id),
                        t.JassTeamTyp,
                        t.JassTeamSpieler.Select(ts => new JassTeamSpieler(
                            new JassTeamSpielerId(ts.Id),
                            new SpielerId(ts.Spieler.Id),
                            ts.Spieler.Name,
                            ts.IstStartSpieler,
                            ts.Position)).ToList())).ToList(),
                    new CoiffeurSpielrundeOptionen(f.DoIncludeRaucherpausen, f.DoIncludeShots))
            );

            return map;
        }
    }
}