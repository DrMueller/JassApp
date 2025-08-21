namespace JassApp.Domain.Jassrunden.Models.Jass
{
    public class JassSuiteInHand
    {
        public IReadOnlyCollection<JassCard> Cards { get; }
        public JassSuite Suite { get; }

        public JassSuiteInHand(JassSuite suite, IReadOnlyCollection<JassCard> cards)
        {
            Suite = suite;
            Cards = cards;
        }
    }
}