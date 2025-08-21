namespace JassApp.Domain.Jassrunden.Models.Jass
{
    public class CardValue
    {
        private static readonly Lazy<IReadOnlyCollection<CardValue>> _allLazy = new(() => new List<CardValue>
        {
            new(CardValueType.Sechs),
            new(CardValueType.Sieben),
            new(CardValueType.Acht),
            new(CardValueType.Neun),
            new(CardValueType.Zehn),
            new(CardValueType.Bube),
            new(CardValueType.Dame),
            new(CardValueType.König),
            new(CardValueType.Ass)
        });

        private CardValue(CardValueType cardValueType)
        {
            CardValueType = cardValueType;
        }

        public static IReadOnlyCollection<CardValue> All => _allLazy.Value;
        public CardValueType CardValueType { get; }

        public override string ToString()
        {
            return CardValueType.ToString();
        }
    }
}