using JassApp.Common.InformationHandling;
using JassApp.Common.LanguageExtensions.Types.Maybes;
using JassApp.Common.LanguageExtensions.Types.Maybes.Implementation;
using JassApp.Common.Logging.Services;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Coiffeur.Specifications;
using JassApp.Domain.Shared.Data.Querying;
using JassApp.Domain.Spieler.Models;
using JassApp.Domain.Spieler.Specifications;

namespace JassApp.Presentation.Areas.Coiffeur.Auswertung
{
    public class AuswertungService(
        IQueryService queryService,
        ILoggingService loggingService)
        : IAuswertungService
    {
        public async Task<(InformationEntries Infos, IReadOnlyCollection<AuswertungSpielerItemModel> Spieler)> LoadSpielerAsync()
        {
            var spieler = await queryService.QueryAsync(new SpielerSpec());
            var map = spieler
                .OrderBy(s => s.Name)
                .Select(s => new AuswertungSpielerItemModel(s.Id.Value, s.Name))
                .ToList();

            return (InformationEntries.Empty, map);
        }

        public async Task<(InformationEntries Infos, SpielrundenAuswertungErgebnisModel Ergebnis)> EvaluateSpielrundenAsync(AuswertungFilterModel filter)
        {
            var validation = ValidateFilter(filter);
            if (validation.HasErrorsOrWarnings)
            {
                return (validation, SpielrundenAuswertungErgebnisModel.Empty);
            }

            var (infos, roundContexts) = await LoadRoundContextsAsync(filter);
            if (infos.HasErrorsOrWarnings)
            {
                return (infos, SpielrundenAuswertungErgebnisModel.Empty);
            }

            var orderedContexts = roundContexts
                .OrderBy(c => c.Runde.GestartetAm)
                .ToList();

            var details = orderedContexts
                .Select(c => new SpielrundenDetailModel(
                    c.Runde.GestartetAm,
                    c.SpielerTeamText,
                    c.GegnerTeamText,
                    c.PunkteSpieler,
                    c.PunkteGegner,
                    c.PunkteDifferenz,
                    c.MaetscheSpieler,
                    c.MaetscheGegner,
                    c.MaetschDifferenz))
                .ToList();

            var summary = new AuswertungSummaryModel(
                details.Count,
                details.Sum(f => f.PunkteSpieler),
                details.Sum(f => f.PunkteGegner),
                details.Sum(f => f.PunkteDifferenz),
                details.Sum(f => f.MaetscheSpieler),
                details.Sum(f => f.MaetscheGegner),
                details.Sum(f => f.MaetschDifferenz));

            var gegnerBilanzen = BuildGegnerBilanzen(filter, orderedContexts);

            return (InformationEntries.Empty, new SpielrundenAuswertungErgebnisModel(details, summary, gegnerBilanzen));
        }

        public async Task<(InformationEntries Infos, TrumpfrundenAuswertungErgebnisModel Ergebnis)> EvaluateTrumpfrundenAsync(AuswertungFilterModel filter)
        {
            var validation = ValidateFilter(filter);
            if (validation.HasErrorsOrWarnings)
            {
                return (validation, TrumpfrundenAuswertungErgebnisModel.Empty);
            }

            var (infos, roundContexts) = await LoadRoundContextsAsync(filter);
            if (infos.HasErrorsOrWarnings)
            {
                return (infos, TrumpfrundenAuswertungErgebnisModel.Empty);
            }

            var orderedContexts = roundContexts
                .OrderBy(c => c.Runde.GestartetAm)
                .ToList();

            var details = new List<TrumpfrundenDetailModel>();

            foreach (var context in orderedContexts)
            {
                var orderedTrumpfrunden = context.Runde.Trumpfrunden
                    .OrderBy(tr => tr.ID.Value)
                    .ToList();

                foreach (var trumpfrunde in orderedTrumpfrunden)
                {
                    if (filter.TrumpfrundeTypFilter.HasValue && trumpfrunde.CoiffeurTrumpf.Typ != filter.TrumpfrundeTypFilter.Value)
                    {
                        continue;
                    }

                    try
                    {
                        var punkteSpielerMaybe = trumpfrunde.CalculatePunktedifferenz(context.SpielerTeamTyp);
                        var punkteGegnerMaybe = trumpfrunde.CalculatePunktedifferenz(context.GegnerTeamTyp);
                        if (!punkteSpielerMaybe.HasValue || !punkteGegnerMaybe.HasValue)
                        {
                            continue;
                        }

                        var punkteSpieler = punkteSpielerMaybe.Value;
                        var punkteGegner = punkteGegnerMaybe.Value;

                        details.Add(new TrumpfrundenDetailModel(
                            context.Runde.GestartetAm,
                            trumpfrunde.CoiffeurTrumpf.Typ,
                            context.SpielerTeamText,
                            context.GegnerTeamText,
                            punkteSpieler,
                            punkteGegner,
                            punkteSpieler - punkteGegner));
                    }
                    catch (Exception ex)
                    {
                        loggingService.LogException(ex);
                    }
                }
            }

            var summary = new TrumpfrundenSummaryModel(
                details.Count,
                details.Sum(f => f.PunkteSpieler),
                details.Sum(f => f.PunkteGegner),
                details.Sum(f => f.PunkteDifferenz));

            return (InformationEntries.Empty, new TrumpfrundenAuswertungErgebnisModel(details, summary));
        }

        private static List<GegnerBilanzModel> BuildGegnerBilanzen(
            AuswertungFilterModel filter,
            IReadOnlyCollection<RoundContext> roundContexts)
        {
            if (filter.GegnerId.HasValue)
            {
                return [];
            }

            var rows = new List<GegnerBilanzAccumulator>();

            foreach (var context in roundContexts)
            {
                var uniqueGegner = context.GegnerSpieler
                    .GroupBy(g => g.SpielerId.Value)
                    .Select(g => g.First())
                    .ToList();

                foreach (var gegner in uniqueGegner)
                {
                    var acc = rows.SingleOrDefault(r => r.GegnerId == gegner.SpielerId.Value);
                    if (acc == null)
                    {
                        acc = new GegnerBilanzAccumulator(gegner.SpielerId.Value, gegner.Name);
                        rows.Add(acc);
                    }

                    acc.Add(context);
                }
            }

            return rows
                .OrderBy(r => r.GegnerName)
                .Select(r => r.ToModel())
                .ToList();
        }

        private async Task<(InformationEntries Infos, IReadOnlyCollection<RoundContext> Contexts)> LoadRoundContextsAsync(AuswertungFilterModel filter)
        {
            var spielerId = new SpielerId(filter.SpielerId!.Value);
            Maybe<SpielerId> gegnerIdMaybe = filter.GegnerId.HasValue
                ? new SpielerId(filter.GegnerId.Value)
                : None.Value;

            Maybe<DateTime> vonMaybe = filter.Von.HasValue
                ? filter.Von.Value.Date
                : None.Value;

            Maybe<DateTime> bisMaybe = filter.Bis.HasValue
                ? filter.Bis.Value.Date.AddDays(1).AddTicks(-1)
                : None.Value;

            Maybe<bool> isJassTrainingslagerMaybe = filter.JasstrainingslagerFilter switch
            {
                JasstrainingslagerFilterTyp.Ja => true,
                JasstrainingslagerFilterTyp.Nein => false,
                _ => None.Value
            };

            var spec = new CoiffeurSpielrundeSpec(
                spielerId,
                None.Value,
                vonMaybe,
                bisMaybe,
                isJassTrainingslagerMaybe,
                true,
                false);

            var runden = await queryService.QueryAsync(spec);
            var contexts = new List<RoundContext>();

            foreach (var runde in runden)
            {
                try
                {
                    if (!runde.WasFinished)
                    {
                        continue;
                    }

                    if (filter.Von.HasValue && runde.GestartetAm < filter.Von.Value.Date)
                    {
                        continue;
                    }

                    if (filter.Bis.HasValue && runde.GestartetAm > filter.Bis.Value.Date.AddDays(1).AddTicks(-1))
                    {
                        continue;
                    }

                    if (filter.JasstrainingslagerFilter == JasstrainingslagerFilterTyp.Ja && !runde.Optionen.IsJassTrainingslager)
                    {
                        continue;
                    }

                    if (filter.JasstrainingslagerFilter == JasstrainingslagerFilterTyp.Nein && runde.Optionen.IsJassTrainingslager)
                    {
                        continue;
                    }

                    var contextMaybe = TryCreateRoundContext(runde, spielerId, gegnerIdMaybe);
                    if (contextMaybe is Some<RoundContext> contextSome)
                    {
                        RoundContext context = contextSome;
                        contexts.Add(context);
                    }
                }
                catch (Exception ex)
                {
                    loggingService.LogException(ex);
                }
            }

            return (InformationEntries.Empty, contexts);
        }

        private static Maybe<RoundContext> TryCreateRoundContext(
            CoiffeurSpielrunde runde,
            SpielerId spielerId,
            Maybe<SpielerId> gegnerIdMaybe)
        {
            var spielerTeam = runde.JassTeams.SingleOrDefault(team => TeamContainsSpieler(team, spielerId));
            if (spielerTeam == null)
            {
                return None.Value;
            }

            var gegnerTeam = runde.JassTeams.SingleOrDefault(team => team.Typ != spielerTeam.Typ);
            if (gegnerTeam == null)
            {
                return None.Value;
            }

            var allSpielerIds = new[]
            {
                spielerTeam.Spieler1.SpielerId.Value,
                spielerTeam.Spieler2.SpielerId.Value,
                gegnerTeam.Spieler1.SpielerId.Value,
                gegnerTeam.Spieler2.SpielerId.Value
            };

            if (allSpielerIds.Distinct().Count() != 4)
            {
                return None.Value;
            }

            if (gegnerIdMaybe is Some<SpielerId> gegnerId)
            {
                SpielerId gegnerValue = gegnerId;
                var hasGegnerInOpposingTeam = TeamContainsSpieler(gegnerTeam, gegnerValue);
                if (!hasGegnerInOpposingTeam)
                {
                    return None.Value;
                }
            }

            var punktetotalSpieler = runde.CalculateTotalPunkte(spielerTeam.Typ);
            var punktetotalGegner = runde.CalculateTotalPunkte(gegnerTeam.Typ);

            var maetscheSpieler = runde.CalculateMaetche(spielerTeam.Typ);
            var maetscheGegner = runde.CalculateMaetche(gegnerTeam.Typ);

            var normalizedSpielerTeamText = string.Join(", ",
                new[] { spielerTeam.Spieler1.Name, spielerTeam.Spieler2.Name }.OrderBy(n => n));
            var normalizedGegnerTeamText = string.Join(", ",
                new[] { gegnerTeam.Spieler1.Name, gegnerTeam.Spieler2.Name }.OrderBy(n => n));

            if (!punktetotalSpieler.Punkte.HasValue || !punktetotalGegner.Punkte.HasValue)
            {
                return None.Value;
            }

            var punkteSpieler = punktetotalSpieler.Punkte.Value;
            var punkteGegner = punktetotalGegner.Punkte.Value;

            return new RoundContext(
                runde,
                spielerTeam.Typ,
                gegnerTeam.Typ,
                normalizedSpielerTeamText,
                normalizedGegnerTeamText,
                punkteSpieler,
                punkteGegner,
                punkteSpieler - punkteGegner,
                maetscheSpieler,
                maetscheGegner,
                maetscheSpieler - maetscheGegner,
                new[] { gegnerTeam.Spieler1, gegnerTeam.Spieler2 });
        }

        private static bool TeamContainsSpieler(JassTeam team, SpielerId spielerId)
        {
            return team.Spieler1.SpielerId == spielerId || team.Spieler2.SpielerId == spielerId;
        }

        private static InformationEntries ValidateFilter(AuswertungFilterModel filter)
        {
            var infos = InformationEntries.CreateNew();

            if (!filter.SpielerId.HasValue)
            {
                infos = infos.AddError("Spieler muss ausgewählt sein.");
            }

            if (filter.SpielerId.HasValue && filter.GegnerId.HasValue && filter.SpielerId.Value == filter.GegnerId.Value)
            {
                infos = infos.AddError("Spieler und Gegner dürfen nicht identisch sein.");
            }

            if (filter.Von.HasValue && filter.Bis.HasValue && filter.Von.Value.Date > filter.Bis.Value.Date)
            {
                infos = infos.AddError("Von darf nicht nach Bis liegen.");
            }

            return infos;
        }

        private sealed class GegnerBilanzAccumulator(int gegnerId, string gegnerName)
        {
            private int Anzahl { get; set; }
            public int GegnerId { get; } = gegnerId;
            public string GegnerName { get; } = gegnerName;
            private int MaetscheGegner { get; set; }
            private int MaetscheSpieler { get; set; }
            private int PunkteGegner { get; set; }
            private int PunkteSpieler { get; set; }

            public void Add(RoundContext context)
            {
                Anzahl += 1;
                PunkteSpieler += context.PunkteSpieler;
                PunkteGegner += context.PunkteGegner;
                MaetscheSpieler += context.MaetscheSpieler;
                MaetscheGegner += context.MaetscheGegner;
            }

            public GegnerBilanzModel ToModel()
            {
                return new GegnerBilanzModel(
                    GegnerName,
                    Anzahl,
                    PunkteSpieler,
                    PunkteGegner,
                    PunkteSpieler - PunkteGegner,
                    MaetscheSpieler,
                    MaetscheGegner,
                    MaetscheSpieler - MaetscheGegner);
            }
        }

        private sealed record RoundContext(
            CoiffeurSpielrunde Runde,
            JassTeamTyp SpielerTeamTyp,
            JassTeamTyp GegnerTeamTyp,
            string SpielerTeamText,
            string GegnerTeamText,
            int PunkteSpieler,
            int PunkteGegner,
            int PunkteDifferenz,
            int MaetscheSpieler,
            int MaetscheGegner,
            int MaetschDifferenz,
            IReadOnlyCollection<JassTeamSpieler> GegnerSpieler);
    }
}
