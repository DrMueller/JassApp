using JassApp.Domain.Jassrunden.Models;

namespace JassApp.Domain.Jassrunden.Services.Implementation
{
    public class JassSpielrundeFactory : IJassSpielrundeFactory
    {
        public JassSpielrunde Create()
        {
            var kartenSet = new JasskartenSet();

            var (hand1, hand2, hand3, hand4) = kartenSet.Ausggeben();

            var player1 = new JasshandSpieler("Player 1", hand1);
            var player2 = new JasshandSpieler("Player 2", hand2);
            var player3 = new JasshandSpieler("Player 3", hand3);
            var player4 = new JasshandSpieler("Player 4", hand4);

            var players = new List<JasshandSpieler>
            {
                player1,
                player2,
                player3,
                player4
            };

            return new JassSpielrunde(players);
        }
    }
}