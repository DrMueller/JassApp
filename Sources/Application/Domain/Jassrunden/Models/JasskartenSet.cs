using JassApp.Domain.Jassrunden.Services.Implementation;

namespace JassApp.Domain.Jassrunden.Models
{
    public class JasskartenSet
    {
        private readonly ICollection<Jasskarte> _jasskarten = new List<Jasskarte>();

        public JasskartenSet()
        {
            var allSuits = JassSuitFactory.CreateAll();
            var allWerte = JasskarteWertFactory.CreateAll();

            foreach (var suit in allSuits)
            {
                foreach (var wert in allWerte)
                {
                    var karte = new Jasskarte(wert, suit);
                    _jasskarten.Add(karte);
                }
            }
        }

        public(JassHand Hand1, JassHand Hand2, JassHand Hand3, JassHand Hand4) Ausggeben()
        {
            var random = new Random();
            var allCards = _jasskarten.ToList();
            var player1Cards = new List<Jasskarte>();
            var player2Cards = new List<Jasskarte>();
            var player3Cards = new List<Jasskarte>();
            var player4Cards = new List<Jasskarte>();

            while (allCards.Count > 0)
            {
                var randomCardIndex = random.Next(0, allCards.Count);
                var card = allCards.ElementAt(randomCardIndex);
                allCards.RemoveAt(randomCardIndex);
                if (player1Cards.Count < 9)
                {
                    player1Cards.Add(card);
                }
                else if (player2Cards.Count < 9)
                {
                    player2Cards.Add(card);
                }
                else if (player3Cards.Count < 9)
                {
                    player3Cards.Add(card);
                }
                else if (player4Cards.Count < 9)
                {
                    player4Cards.Add(card);
                }
            }

            return (new JassHand(player1Cards), new JassHand(player2Cards), new JassHand(player3Cards), new JassHand(player4Cards));
        }
    }
}