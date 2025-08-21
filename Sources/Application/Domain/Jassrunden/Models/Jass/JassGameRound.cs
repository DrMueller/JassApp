using JassApp.Common.LanguageExtensions.Invariance;

namespace JassApp.Domain.Jassrunden.Models.Jass
{
    public class JassGameRound
    {
        public IReadOnlyCollection<JassPlayer> JassPlayers { get; }

        public JassGameRound(IReadOnlyCollection<JassPlayer> jassPlayers)
        {
            Guard.ObjectNotNull(() => jassPlayers);

            if (jassPlayers.Count != 4)
            {
                throw new ArgumentException("Only exactly 4 players allowed.");
            }

            JassPlayers = jassPlayers;
        }

        public IReadOnlyCollection<JassPlayerWithJassSuiteInHand> GetPlayersWithSuiteCombinations(int amountOfSameSuite)
        {
            var result = new List<JassPlayerWithJassSuiteInHand>();

            foreach (var player in JassPlayers)
            {
                var suitesWithAmount = player.JassHand.SuitesInHand.Where(f => f.Cards.Count == amountOfSameSuite);
                result.AddRange(suitesWithAmount.Select(s => new JassPlayerWithJassSuiteInHand(player, s)));
            }

            return result;
        }
    }
}