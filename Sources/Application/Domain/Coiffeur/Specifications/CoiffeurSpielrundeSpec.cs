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
        private readonly Maybe<SpielerId> _gegnerIdMaybe = None.Value;
        private readonly Maybe<bool> _isJassTrainingslagerMaybe = None.Value;
        private readonly bool _onlyFinished;
        private readonly bool _skipInvalidTeamData;
        private readonly Maybe<SpielerId> _spielerIdMaybe = None.Value;
        private readonly Maybe<DateTime> _startedAfterOrAtMaybe = None.Value;
        private readonly Maybe<DateTime> _startedBeforeOrAtMaybe = None.Value;

        public CoiffeurSpielrundeSpec()
        {
        }

        public CoiffeurSpielrundeSpec(CoiffeurSpielrundeId id)
        {
            _idMaybe = id;
        }

        public CoiffeurSpielrundeSpec(
            SpielerId spielerId,
            Maybe<SpielerId> gegnerIdMaybe,
            Maybe<DateTime> startedAfterOrAtMaybe,
            Maybe<DateTime> startedBeforeOrAtMaybe,
            Maybe<bool> isJassTrainingslagerMaybe,
            bool onlyFinished,
            bool skipInvalidTeamData = true)
        {
            _spielerIdMaybe = spielerId;
            _gegnerIdMaybe = gegnerIdMaybe;
            _startedAfterOrAtMaybe = startedAfterOrAtMaybe;
            _startedBeforeOrAtMaybe = startedBeforeOrAtMaybe;
            _isJassTrainingslagerMaybe = isJassTrainingslagerMaybe;
            _onlyFinished = onlyFinished;
            _skipInvalidTeamData = skipInvalidTeamData;
        }

        public IQueryable<CoiffeurSpielrunde> Apply(IQueryBase qryProvider)
        {
            var qry = qryProvider.Query<CoiffeurSpielrundeTable>();
            qry = qry.WhereOptional(
                _idMaybe,
                f => q => q.Id == f.Value);

            qry = qry.WhereOptional(
                _spielerIdMaybe,
                f => q => q.JassTeams.Any(t => t.JassTeamSpieler.Any(s => s.SpielerId == f.Value)));

            qry = qry.WhereOptional(
                _startedAfterOrAtMaybe,
                f => q => q.GestartetAm >= f);

            qry = qry.WhereOptional(
                _startedBeforeOrAtMaybe,
                f => q => q.GestartetAm <= f);

            qry = qry.WhereOptional(
                _isJassTrainingslagerMaybe,
                f => q => q.IsJassTrainingslager == f);

            if (_skipInvalidTeamData)
            {
                qry = qry.Where(q =>
                    q.Trumpfrunden.Any()
                    && q.JassTeams.Count == 2
                    && q.JassTeams.All(t => t.JassTeamSpieler.Count == 2));
            }

            if (_onlyFinished)
            {
                qry = qry.Where(q => q.Trumpfrunden.All(tr => tr.ResultatTeam1.HasValue && tr.ResultatTeam2.HasValue));
            }

            if (_spielerIdMaybe is Some<SpielerId> spielerIdMaybe && _gegnerIdMaybe is Some<SpielerId> gegnerIdMaybe)
            {
                SpielerId spielerId = spielerIdMaybe;
                SpielerId gegnerId = gegnerIdMaybe;

                qry = qry.Where(q =>
                    q.JassTeams.Any(t =>
                        t.JassTeamSpieler.Any(s => s.SpielerId == spielerId.Value)
                        && q.JassTeams.Any(ot => ot.JassTeamTyp != t.JassTeamTyp
                                                 && ot.JassTeamSpieler.Any(os => os.SpielerId == gegnerId.Value))));
            }

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
                    new CoiffeurSpielrundeOptionen(f.DoIncludeRaucherpausen, f.DoIncludeShots, f.IsJassTrainingslager))
            );

            return map;
        }
    }
}
