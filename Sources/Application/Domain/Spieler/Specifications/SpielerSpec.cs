using JassApp.Common.LanguageExtensions.Types.Maybes;
using JassApp.Common.LanguageExtensions.Types.Maybes.Implementation;
using JassApp.DataAccess;
using JassApp.DataAccess.Tables;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Shared.Data.Querying;
using JassApp.Domain.Spieler.Models;

namespace JassApp.Domain.Spieler.Specifications
{
    public class SpielerSpec : IQuerySpecification<Models.Spieler>
    {
        private readonly Maybe<SpielerId> _idMaybe = None.Value;

        public SpielerSpec(SpielerId id)
        {
            _idMaybe = id;
        }

        public SpielerSpec()
        {
        }

        public IQueryable<Models.Spieler> Apply(IQueryBase qryProvider)
        {
            var qry = qryProvider.Query<SpielerTable>();
            qry = qry.WhereOptional(_idMaybe, f => q => q.Id == f.Value);

            return qry
                .Select(f => new Models.Spieler(
                    new SpielerId(f.Id),
                    f.Name,
                    f.JassTeamSpieler.Select(jts => new JassTeamId(jts.JassTeamId)).ToList()));
        }
    }
}