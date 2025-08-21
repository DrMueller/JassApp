using JassApp.Domain.Jassrunden.Models.Jass;
using JassApp.Domain.Jassrunden.Services.Servants;
using Mmu.Jsc.Domain.Areas.Services;

namespace JassApp.Domain.Jassrunden.Services.Implementation
{
    public class JassGameRoundFactory : IJassGameRoundFactory
    {
        public JassGameRound CreateOneRound()
        {
            var allCards = new List<JassCard>(JassCard.CompleteSet);

            var player1 = new JassPlayer("Player 1");
            var player2 = new JassPlayer("Player 2");
            var player3 = new JassPlayer("Player 3");
            var player4 = new JassPlayer("Player 4");

            var players = new List<JassPlayer>
            {
                player1,
                player2,
                player3,
                player4
            };

            while (allCards.Count > 0)
            {
                var randomCardIndex = RandomSingleton.Instance.Next(allCards.Count);
                var card = allCards.ElementAt(randomCardIndex);
                allCards.RemoveAt(randomCardIndex);

                var playerWithoutNineCards = players.First(f => f.JassHand.Cards.Count < 9);
                playerWithoutNineCards.JassHand.AddCard(card);
            }

            player1.AssignPartner(player3);
            player2.AssignPartner(player4);
            player3.AssignPartner(player1);
            player4.AssignPartner(player2);

            return new JassGameRound(players);
        }
    }
}