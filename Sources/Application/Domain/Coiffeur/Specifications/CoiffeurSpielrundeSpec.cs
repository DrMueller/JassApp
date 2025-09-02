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
        private Maybe<CoiffeurSpielrundeId> _idMaybe = None.Value;

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
                        tr.ResultatTeam1,
                        tr.ResultatTeam2)).ToList(),
                    f.JassTeams.Select(t => new JassTeam(
                        new JassTeamId(t.Id),
                        t.JassTeamTyp,
                        t.JassTeamSpieler.Select(ts => new JassTeamSpieler(
                            new JassTeamSpielerId(ts.Id),
                            new SpielerId(ts.Spieler.Id),
                            ts.Spieler.Name,
                            ts.IstStartSpieler,
                            ts.Position)).ToList())).ToList())
            );

            return map;
        }
    }
}