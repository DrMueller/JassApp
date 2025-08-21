namespace JassApp.Domain.Jassrunden.Models.Jass
{
    public class JassHand
    {
        private List<JassCard> _cards;
        public IReadOnlyCollection<JassCard> Cards => _cards;

        public IReadOnlyCollection<JassSuiteInHand> SuitesInHand
        {
            get
            {
                var grp = Cards.GroupBy(c => c.JassSuite).OrderByDescending(c => c.Count());
                var dict = grp.ToDictionary(key => key.Key, cards => (IReadOnlyCollection<JassCard>)cards.ToList());
                var result = dict.Select(d => new JassSuiteInHand(d.Key, d.Value)).ToList();
                return result;
            }
        }

        public JassHand()
        {
            _cards = new List<JassCard>();
        }

        public void AddCard(JassCard card)
        {
            _cards.Add(card);
        }
    }
}