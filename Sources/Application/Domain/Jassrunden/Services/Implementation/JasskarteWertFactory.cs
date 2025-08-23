using JassApp.Domain.Jassrunden.Models;

namespace JassApp.Domain.Jassrunden.Services.Implementation
{
    public static class JasskarteWertFactory
    {
        private static readonly Lazy<IReadOnlyCollection<JasskarteWert>> _allLazy = new(() => new List<JasskarteWert>
        {
            new(JasskarteWertTyp.Sechs, "6"),
            new(JasskarteWertTyp.Sieben, "7"),
            new(JasskarteWertTyp.Acht, "8"),
            new(JasskarteWertTyp.Neun, "9"),
            new(JasskarteWertTyp.Zehn, "10"),
            new(JasskarteWertTyp.Bube, "B"),
            new(JasskarteWertTyp.Dame, "D"),
            new(JasskarteWertTyp.König, "K"),
            new(JasskarteWertTyp.Ass, "A")
        });

        public static IReadOnlyCollection<JasskarteWert> CreateAll()
        {
            return _allLazy.Value;
        }
    }
}