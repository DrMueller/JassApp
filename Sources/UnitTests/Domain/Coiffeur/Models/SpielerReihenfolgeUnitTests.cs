//using FluentAssertions;
//using JassApp.Domain.Coiffeur.Models;
//using JassApp.UnitTests.TestingInfrastructure.DomainModelBuilders;
//using Xunit;

//namespace JassApp.UnitTests.Domain.Coiffeur.Models
//{
//    public class SpielerReihenfolgeUnitTests
//    {
//        [Theory]
//        [InlineData(0, "T1S1")]
//        [InlineData(1, "T2S1")]
//        [InlineData(2, "T1S2")]
//        [InlineData(3, "T2S2")]
//        [InlineData(4, "T1S1")]
//        public void CalculateActiveSpieler_WhileBothTeamsStillPlay_RotatesAll4Players(int playedRounds, string expected)
//        {
//            var (team1, team2) = JassTeamTestBuilder.Create(startSpielerPos: 0);

//            // Create trump rounds such that Sum(AmountOfResultate) == playedRounds,
//            // but neither team has played all rounds.
//            var trumpfrunden = new List<CoiffeurTrumpfrunde>
//            {
//                new(new TrumpfrundeId(0), 1, CoiffeurTrumpf.Herz, resultatTeam1: "10", resultatTeam2: null),
//                new(new TrumpfrundeId(1), 1, CoiffeurTrumpf.Egge, resultatTeam1: null, resultatTeam2: null),
//                new(new TrumpfrundeId(2), 1, CoiffeurTrumpf.Kreuz, resultatTeam1: null, resultatTeam2: null)
//            };

//            // Add additional results to reach the wanted playedRounds without finishing any team.
//            // TrumpfrundeId(0) already adds 1 result for Team1.
//            if (playedRounds >= 2)
//            {
//                trumpfrunden[1][JassTeamTyp.Team2].RawInput = "5";
//            }

//            if (playedRounds >= 3)
//            {
//                trumpfrunden[2][JassTeamTyp.Team1].RawInput = "8";
//            }

//            if (playedRounds >= 4)
//            {
//                trumpfrunden[2][JassTeamTyp.Team2].RawInput = "7";
//            }

//            var sut = new SpielerReihenfolge([team1, team2], trumpfrunden);
//            var active = sut.CalculateActiveSpieler(JassTeamTyp.Team1);

//            var expectedSpieler = expected switch
//            {
//                "T1S1" => team1.Spieler1,
//                "T1S2" => team1.Spieler2,
//                "T2S1" => team2.Spieler1,
//                "T2S2" => team2.Spieler2,
//                _ => throw new ArgumentOutOfRangeException(nameof(expected), expected, null)
//            };

//            active.Should().BeSameAs(expectedSpieler);
//        }

//        [Fact]
//        public void CalculateActiveSpieler_WhenOpposingTeamFinished_OpposingTeamCannotBeActiveAnymore()
//        {
//            var (team1, team2) = JassTeamTestBuilder.Create(startSpielerPos: 1); // start player = Team2 Spieler1

//            // Team2 finished all rounds, Team1 not.
//            var trumpfrunden = new List<CoiffeurTrumpfrunde>
//            {
//                new(new TrumpfrundeId(0), 1, CoiffeurTrumpf.Herz, resultatTeam1: null, resultatTeam2: "10"),
//                new(new TrumpfrundeId(1), 1, CoiffeurTrumpf.Egge, resultatTeam1: null, resultatTeam2: "5"),
//                new(new TrumpfrundeId(2), 1, CoiffeurTrumpf.Kreuz, resultatTeam1: null, resultatTeam2: "7")
//            };

//            // playedRounds = 3, opposing (Team2) is finished => only Team1 rotates.
//            // Team1 has no start player => startIndexInTeam defaults to Spieler1.
//            // (0 + 3) % 2 = 1 => Team1 Spieler2
//            var sut = new SpielerReihenfolge([team1, team2], trumpfrunden);
//            var active = sut.CalculateActiveSpieler(JassTeamTyp.Team1);

//            active.Should().BeSameAs(team1.Spieler2);
//        }
//    }
//}