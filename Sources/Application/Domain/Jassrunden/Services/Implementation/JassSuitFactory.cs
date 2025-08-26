using JassApp.Domain.Jassrunden.Models;

namespace JassApp.Domain.Jassrunden.Services.Implementation
{
    public static class JassSuitFactory
    {
        private static readonly Lazy<IReadOnlyCollection<JassSuit>> _allLazy = new(() => new List<JassSuit>
        {
            new(JassSuitType.Ecken, "#c52a2a", "♦"),
            new(JassSuitType.Herz, "#c52a2a", "♥"),
            new(JassSuitType.Kreuz, "#1f5faa", "♣"),
            new(JassSuitType.Schaufeln, "#2f7d32", "♠")
        });

        public static IReadOnlyCollection<JassSuit> CreateAll()
        {
            return _allLazy.Value;
        }
    }
}