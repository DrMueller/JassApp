using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using JassApp.Common.Logging.Services;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Shared.Data.Querying;
using JassApp.Domain.Spieler.Models;
using JassApp.Presentation.Areas.Coiffeur.Auswertung;
using Moq;
using Xunit;

namespace JassApp.UnitTests.Presentation.Areas.Coiffeur.Auswertung
{
    public class AuswertungServiceUnitTests
    {
        private readonly Mock<ILoggingService> _loggingMock = new();
        private readonly Mock<IQueryService> _queryMock = new();

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateSpielrundenAsync_FindsRoundWithSelectedSpieler()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var gegner = CreateSpieler(2, "Stefan");
            var round = CreateFinishedRound(DateTime.Today, spieler, CreateSpieler(3, "Peter"), gegner, CreateSpieler(4, "Thomas"));

            SetupRounds(round);
            var sut = CreateSut();

            var (_, result) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value });

            result.Details.Should().HaveCount(1);
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateSpielrundenAsync_DoesNotFindRoundWithoutSelectedSpieler()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var round = CreateFinishedRound(DateTime.Today, CreateSpieler(2, "Stefan"), CreateSpieler(3, "Peter"), CreateSpieler(4, "Thomas"), CreateSpieler(5, "Anna"));

            SetupRounds(round);
            var sut = CreateSut();

            var (_, result) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value });

            result.Details.Should().BeEmpty();
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateSpielrundenAsync_GegnerInOpposingTeamIsFound()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var gegner = CreateSpieler(2, "Stefan");
            var round = CreateFinishedRound(DateTime.Today, spieler, CreateSpieler(3, "Peter"), gegner, CreateSpieler(4, "Thomas"));

            SetupRounds(round);
            var sut = CreateSut();

            var (_, result) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value, GegnerId = gegner.Id.Value });

            result.Details.Should().HaveCount(1);
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateSpielrundenAsync_GegnerInSameTeamIsIgnored()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var sameTeamSpieler = CreateSpieler(2, "Stefan");
            var round = CreateFinishedRound(DateTime.Today, spieler, sameTeamSpieler, CreateSpieler(3, "Peter"), CreateSpieler(4, "Thomas"));

            SetupRounds(round);
            var sut = CreateSut();

            var (_, result) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value, GegnerId = sameTeamSpieler.Id.Value });

            result.Details.Should().BeEmpty();
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateSpielrundenAsync_OtherGegnerIsNotFound()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var round = CreateFinishedRound(DateTime.Today, spieler, CreateSpieler(3, "Peter"), CreateSpieler(2, "Stefan"), CreateSpieler(4, "Thomas"));

            SetupRounds(round);
            var sut = CreateSut();

            var (_, result) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value, GegnerId = 999 });

            result.Details.Should().BeEmpty();
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateSpielrundenAsync_NoGegnerIncludesAllGegner()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var round = CreateFinishedRound(DateTime.Today, spieler, CreateSpieler(3, "Peter"), CreateSpieler(2, "Stefan"), CreateSpieler(4, "Thomas"));

            SetupRounds(round);
            var sut = CreateSut();

            var (_, result) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value });

            result.GegnerBilanzen.Should().HaveCount(2);
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateSpielrundenAsync_PerspectiveIsNormalizedForTeam1AndTeam2()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var roundAsTeam1 = CreateFinishedRound(DateTime.Today, spieler, CreateSpieler(2, "A"), CreateSpieler(3, "B"), CreateSpieler(4, "C"));
            var roundAsTeam2 = CreateFinishedRound(DateTime.Today.AddDays(1), CreateSpieler(3, "B"), CreateSpieler(4, "C"), spieler, CreateSpieler(2, "A"));

            SetupRounds(roundAsTeam1, roundAsTeam2);
            var sut = CreateSut();

            var (_, result) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value });

            result.Details.Should().HaveCount(2);
            result.Details.All(f => f.SpielerTeam.Contains("Matthias")).Should().BeTrue();
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateSpielrundenAsync_FinishedRoundsIncludedUnfinishedIgnored()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var finished = CreateFinishedRound(DateTime.Today, spieler, CreateSpieler(2, "A"), CreateSpieler(3, "B"), CreateSpieler(4, "C"));
            var unfinished = CreateUnfinishedRound(DateTime.Today.AddDays(1), spieler, CreateSpieler(5, "D"), CreateSpieler(6, "E"), CreateSpieler(7, "F"));

            SetupRounds(finished, unfinished);
            var sut = CreateSut();

            var (_, result) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value });

            result.Details.Should().HaveCount(1);
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateSpielrundenAsync_DateFiltersAreInclusive()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var d1 = new DateTime(2026, 3, 10);
            var d2 = new DateTime(2026, 3, 11);
            var d3 = new DateTime(2026, 3, 12);

            SetupRounds(
                CreateFinishedRound(d1, spieler, CreateSpieler(2, "A"), CreateSpieler(3, "B"), CreateSpieler(4, "C")),
                CreateFinishedRound(d2, spieler, CreateSpieler(5, "D"), CreateSpieler(6, "E"), CreateSpieler(7, "F")),
                CreateFinishedRound(d3, spieler, CreateSpieler(8, "G"), CreateSpieler(9, "H"), CreateSpieler(10, "I")));

            var sut = CreateSut();

            var (_, all) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value });
            var (_, fromOnly) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value, Von = d2 });
            var (_, toOnly) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value, Bis = d2 });
            var (_, range) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value, Von = d1, Bis = d2 });

            all.Details.Should().HaveCount(3);
            fromOnly.Details.Should().HaveCount(2);
            toOnly.Details.Should().HaveCount(2);
            range.Details.Should().HaveCount(2);
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateSpielrundenAsync_VonGreaterBisIsValidationError()
        {
            SetupRounds();
            var sut = CreateSut();

            var (infos, result) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel
            {
                SpielerId = 1,
                Von = new DateTime(2026, 3, 12),
                Bis = new DateTime(2026, 3, 11)
            });

            infos.HasErrors.Should().BeTrue();
            result.Should().Be(SpielrundenAuswertungErgebnisModel.Empty);
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateSpielrundenAsync_JasstrainingslagerFilterWorks()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var jtl = CreateFinishedRound(DateTime.Today, spieler, CreateSpieler(2, "A"), CreateSpieler(3, "B"), CreateSpieler(4, "C"), true);
            var nonJtl = CreateFinishedRound(DateTime.Today.AddDays(1), spieler, CreateSpieler(5, "D"), CreateSpieler(6, "E"), CreateSpieler(7, "F"));

            SetupRounds(jtl, nonJtl);
            var sut = CreateSut();

            var (_, all) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value, JasstrainingslagerFilter = JasstrainingslagerFilterTyp.Alle });
            var (_, yes) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value, JasstrainingslagerFilter = JasstrainingslagerFilterTyp.Ja });
            var (_, no) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value, JasstrainingslagerFilter = JasstrainingslagerFilterTyp.Nein });

            all.Details.Should().HaveCount(2);
            yes.Details.Should().HaveCount(1);
            no.Details.Should().HaveCount(1);
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateTrumpfrundenAsync_FilterByEnumTypeWorks()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var round = CreateFinishedRound(DateTime.Today, spieler, CreateSpieler(2, "A"), CreateSpieler(3, "B"), CreateSpieler(4, "C"));

            SetupRounds(round);
            var sut = CreateSut();

            var (_, all) = await sut.EvaluateTrumpfrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value });
            var (_, onlyHerz) = await sut.EvaluateTrumpfrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value, TrumpfrundeTypFilter = CoiffeurTrumpfTyp.Herz });

            all.Details.Should().HaveCount(2);
            onlyHerz.Details.Should().ContainSingle();
            onlyHerz.Details.Single().TrumpfrundeTyp.Should().Be(CoiffeurTrumpfTyp.Herz);
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateSpielrundenAsync_CombinedFilters_GegnerDateJtlAreApplied()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var gegner = CreateSpieler(2, "Stefan");
            var targetDate = new DateTime(2026, 3, 11);

            var matching = CreateFinishedRound(targetDate, spieler, CreateSpieler(3, "A"), gegner, CreateSpieler(4, "B"), true);
            var wrongDate = CreateFinishedRound(targetDate.AddDays(3), spieler, CreateSpieler(5, "C"), gegner, CreateSpieler(6, "D"), true);
            var wrongGegner = CreateFinishedRound(targetDate, spieler, CreateSpieler(7, "E"), CreateSpieler(99, "Other"), CreateSpieler(8, "F"), true);
            var wrongJtl = CreateFinishedRound(targetDate, spieler, CreateSpieler(9, "G"), gegner, CreateSpieler(10, "H"));

            SetupRounds(matching, wrongDate, wrongGegner, wrongJtl);
            var sut = CreateSut();

            var (_, result) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel
            {
                SpielerId = spieler.Id.Value,
                GegnerId = gegner.Id.Value,
                Von = targetDate,
                Bis = targetDate,
                JasstrainingslagerFilter = JasstrainingslagerFilterTyp.Ja
            });

            result.Details.Should().HaveCount(1);
            result.Details.Single().GestartetAm.Date.Should().Be(targetDate.Date);
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateTrumpfrundenAsync_CombinedFilters_GegnerDateJtlAndTypeAreApplied()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var gegner = CreateSpieler(2, "Stefan");
            var targetDate = new DateTime(2026, 3, 11);

            var matching = CreateFinishedRound(targetDate, spieler, CreateSpieler(3, "A"), gegner, CreateSpieler(4, "B"), true);
            var wrongDate = CreateFinishedRound(targetDate.AddDays(3), spieler, CreateSpieler(5, "C"), gegner, CreateSpieler(6, "D"), true);
            var wrongGegner = CreateFinishedRound(targetDate, spieler, CreateSpieler(7, "E"), CreateSpieler(99, "Other"), CreateSpieler(8, "F"), true);
            var wrongJtl = CreateFinishedRound(targetDate, spieler, CreateSpieler(9, "G"), gegner, CreateSpieler(10, "H"));

            SetupRounds(matching, wrongDate, wrongGegner, wrongJtl);
            var sut = CreateSut();

            var (_, result) = await sut.EvaluateTrumpfrundenAsync(new AuswertungFilterModel
            {
                SpielerId = spieler.Id.Value,
                GegnerId = gegner.Id.Value,
                Von = targetDate,
                Bis = targetDate,
                JasstrainingslagerFilter = JasstrainingslagerFilterTyp.Ja,
                TrumpfrundeTypFilter = CoiffeurTrumpfTyp.Herz
            });

            result.Details.Should().ContainSingle();
            result.Details.Single().GestartetAm.Date.Should().Be(targetDate.Date);
            result.Details.Single().TrumpfrundeTyp.Should().Be(CoiffeurTrumpfTyp.Herz);
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateSpielrundenAsync_WithGegnerWithoutDateStillReturnsDetails()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var gegner = CreateSpieler(2, "Stefan");

            var matching1 = CreateFinishedRound(new DateTime(2026, 3, 10), spieler, CreateSpieler(3, "A"), gegner, CreateSpieler(4, "B"), true);
            var matching2 = CreateFinishedRound(new DateTime(2026, 3, 11), spieler, CreateSpieler(5, "C"), gegner, CreateSpieler(6, "D"));
            var wrongGegner = CreateFinishedRound(new DateTime(2026, 3, 12), spieler, CreateSpieler(7, "E"), CreateSpieler(99, "Other"), CreateSpieler(8, "F"), true);

            SetupRounds(matching1, matching2, wrongGegner);
            var sut = CreateSut();

            var (_, result) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel
            {
                SpielerId = spieler.Id.Value,
                GegnerId = gegner.Id.Value
            });

            result.Details.Should().HaveCount(2);
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateTrumpfrundenAsync_WithGegnerWithoutDateStillReturnsDetails()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var gegner = CreateSpieler(2, "Stefan");

            var matching1 = CreateFinishedRound(new DateTime(2026, 3, 10), spieler, CreateSpieler(3, "A"), gegner, CreateSpieler(4, "B"), true);
            var matching2 = CreateFinishedRound(new DateTime(2026, 3, 11), spieler, CreateSpieler(5, "C"), gegner, CreateSpieler(6, "D"));
            var wrongGegner = CreateFinishedRound(new DateTime(2026, 3, 12), spieler, CreateSpieler(7, "E"), CreateSpieler(99, "Other"), CreateSpieler(8, "F"), true);

            SetupRounds(matching1, matching2, wrongGegner);
            var sut = CreateSut();

            var (_, result) = await sut.EvaluateTrumpfrundenAsync(new AuswertungFilterModel
            {
                SpielerId = spieler.Id.Value,
                GegnerId = gegner.Id.Value
            });

            result.Details.Should().HaveCount(4);
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateSpielrundenAsync_EmptyStateIsValid()
        {
            SetupRounds();
            var sut = CreateSut();

            var (infos, result) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = 1 });

            infos.HasErrorsOrWarnings.Should().BeFalse();
            result.Summary.Should().Be(AuswertungSummaryModel.Empty);
            result.Details.Should().BeEmpty();
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateSpielrundenAsync_InvalidDataSetIsSkipped()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var valid = CreateFinishedRound(DateTime.Today, spieler, CreateSpieler(2, "A"), CreateSpieler(3, "B"), CreateSpieler(4, "C"));
            var invalid = CreateInvalidRound(DateTime.Today.AddDays(1), spieler);

            SetupRounds(valid, invalid);
            var sut = CreateSut();

            var (_, result) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value });

            result.Details.Should().HaveCount(1);
        }

        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public async Task EvaluateSpielrundenAsync_GegnerBilanzGroupsBySpielerIdAndSortedByName()
        {
            var spieler = CreateSpieler(1, "Matthias");
            var gegnerWithSameName1 = CreateSpieler(2, "Alex");
            var gegnerWithSameName2 = CreateSpieler(3, "Alex");
            var zed = CreateSpieler(4, "Zed");

            SetupRounds(
                CreateFinishedRound(DateTime.Today, spieler, CreateSpieler(5, "P"), gegnerWithSameName1, zed),
                CreateFinishedRound(DateTime.Today.AddDays(1), spieler, CreateSpieler(6, "Q"), gegnerWithSameName2, zed));

            var sut = CreateSut();
            var (_, result) = await sut.EvaluateSpielrundenAsync(new AuswertungFilterModel { SpielerId = spieler.Id.Value });

            result.GegnerBilanzen.Count(f => f.Gegner == "Alex").Should().Be(2);
            result.GegnerBilanzen.Should().BeInAscendingOrder(f => f.Gegner);
        }

        private static CoiffeurSpielrunde CreateFinishedRound(
            DateTime startedAt,
            Spieler team1S1,
            Spieler team1S2,
            Spieler team2S1,
            Spieler team2S2,
            bool isJtl = false)
        {
            var trumpfrunden = new List<CoiffeurTrumpfrunde>
            {
                new(new TrumpfrundeId(1), 1, CoiffeurTrumpf.Herz,
                    new TrumpfrundeResultat(JassTeamTyp.Team1, 100, true, false),
                    new TrumpfrundeResultat(JassTeamTyp.Team2, 80, false, false)),
                new(new TrumpfrundeId(2), 2, CoiffeurTrumpf.Kreuz,
                    new TrumpfrundeResultat(JassTeamTyp.Team1, 120, false, false),
                    new TrumpfrundeResultat(JassTeamTyp.Team2, 100, false, false))
            };

            var team1 = new JassTeam(new JassTeamId(1), JassTeamTyp.Team1,
            [
                new JassTeamSpieler(new JassTeamSpielerId(1), team1S1.Id, team1S1.Name, true, JassTeamSpielerPosition.Spieler1),
                new JassTeamSpieler(new JassTeamSpielerId(2), team1S2.Id, team1S2.Name, false, JassTeamSpielerPosition.Spieler2)
            ]);

            var team2 = new JassTeam(new JassTeamId(2), JassTeamTyp.Team2,
            [
                new JassTeamSpieler(new JassTeamSpielerId(3), team2S1.Id, team2S1.Name, false, JassTeamSpielerPosition.Spieler1),
                new JassTeamSpieler(new JassTeamSpielerId(4), team2S2.Id, team2S2.Name, false, JassTeamSpielerPosition.Spieler2)
            ]);

            return new CoiffeurSpielrunde(
                new CoiffeurSpielrundeId(1),
                startedAt,
                10,
                trumpfrunden,
                [team1, team2],
                new CoiffeurSpielrundeOptionen(false, false, isJtl));
        }

        private static CoiffeurSpielrunde CreateUnfinishedRound(DateTime startedAt, Spieler team1S1, Spieler team1S2, Spieler team2S1, Spieler team2S2)
        {
            var trumpfrunden = new List<CoiffeurTrumpfrunde>
            {
                new(new TrumpfrundeId(1), 1, CoiffeurTrumpf.Herz,
                    new TrumpfrundeResultat(JassTeamTyp.Team1, 100, false, false),
                    new TrumpfrundeResultat(JassTeamTyp.Team2, null, false, false))
            };

            var team1 = new JassTeam(new JassTeamId(1), JassTeamTyp.Team1,
            [
                new JassTeamSpieler(new JassTeamSpielerId(1), team1S1.Id, team1S1.Name, true, JassTeamSpielerPosition.Spieler1),
                new JassTeamSpieler(new JassTeamSpielerId(2), team1S2.Id, team1S2.Name, false, JassTeamSpielerPosition.Spieler2)
            ]);

            var team2 = new JassTeam(new JassTeamId(2), JassTeamTyp.Team2,
            [
                new JassTeamSpieler(new JassTeamSpielerId(3), team2S1.Id, team2S1.Name, false, JassTeamSpielerPosition.Spieler1),
                new JassTeamSpieler(new JassTeamSpielerId(4), team2S2.Id, team2S2.Name, false, JassTeamSpielerPosition.Spieler2)
            ]);

            return new CoiffeurSpielrunde(
                new CoiffeurSpielrundeId(2),
                startedAt,
                10,
                trumpfrunden,
                [team1, team2],
                new CoiffeurSpielrundeOptionen(false, false, false));
        }

        private static CoiffeurSpielrunde CreateInvalidRound(DateTime startedAt, Spieler spieler)
        {
            var trumpfrunden = new List<CoiffeurTrumpfrunde>
            {
                new(new TrumpfrundeId(1), 1, CoiffeurTrumpf.Herz,
                    new TrumpfrundeResultat(JassTeamTyp.Team1, 100, false, false),
                    new TrumpfrundeResultat(JassTeamTyp.Team2, 80, false, false))
            };

            var team1 = new JassTeam(new JassTeamId(1), JassTeamTyp.Team1,
            [
                new JassTeamSpieler(new JassTeamSpielerId(1), spieler.Id, spieler.Name, true, JassTeamSpielerPosition.Spieler1),
                new JassTeamSpieler(new JassTeamSpielerId(2), spieler.Id, spieler.Name, false, JassTeamSpielerPosition.Spieler2)
            ]);

            var team2 = new JassTeam(new JassTeamId(2), JassTeamTyp.Team2,
            [
                new JassTeamSpieler(new JassTeamSpielerId(3), CreateSpieler(9, "X").Id, "X", false, JassTeamSpielerPosition.Spieler1),
                new JassTeamSpieler(new JassTeamSpielerId(4), CreateSpieler(10, "Y").Id, "Y", false, JassTeamSpielerPosition.Spieler2)
            ]);

            return new CoiffeurSpielrunde(
                new CoiffeurSpielrundeId(3),
                startedAt,
                10,
                trumpfrunden,
                [team1, team2],
                new CoiffeurSpielrundeOptionen(false, false, false));
        }

        private AuswertungService CreateSut()
        {
            return new AuswertungService(_queryMock.Object, _loggingMock.Object);
        }

        private static Spieler CreateSpieler(int id, string name)
        {
            return new Spieler(new SpielerId(id), name, []);
        }

        private void SetupRounds(params CoiffeurSpielrunde[] rounds)
        {
            _queryMock
                .Setup(f => f.QueryAsync(It.IsAny<global::JassApp.Domain.Coiffeur.Specifications.CoiffeurSpielrundeSpec>()))
                .ReturnsAsync(rounds);

            _queryMock
                .Setup(f => f.QueryAsync(It.IsAny<global::JassApp.Domain.Spieler.Specifications.SpielerSpec>()))
                .ReturnsAsync(new List<Spieler>());
        }
    }
}
