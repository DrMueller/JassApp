using JassApp.Common.LanguageExtensions.Invariance;

namespace JassApp.Domain.Jassrunden.Models.Jass
{
    public record JassCard
    {
        private static readonly Lazy<IReadOnlyCollection<JassCard>> _completeSetLazy = new(() => { return CardValue.All.SelectMany(cardValue => { return JassSuite.All.Select(jassSuite => new JassCard(cardValue, jassSuite)); }).ToList(); });

        public JassCard(CardValue cardValue, JassSuite jassSuite)
        {
            Guard.ObjectNotNull(() => cardValue);
            Guard.ObjectNotNull(() => jassSuite);

            CardValue = cardValue;
            JassSuite = jassSuite;
        }

        public static IReadOnlyCollection<JassCard> CompleteSet => _completeSetLazy.Value;
        public CardValue CardValue { get; }
        public JassSuite JassSuite { get; }

        public override string ToString()
        {
            return string.Concat(JassSuite.ToString(), " ", CardValue.ToString());
        }
    }
}