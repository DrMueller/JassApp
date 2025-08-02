using JassApp.Common.LanguageExtensions.Invariance;

namespace JassApp.Domain.Models
{
    public record SpielerId(int Value);

    public class Spieler
    {
        public Spieler(
            SpielerId id,
            string name)
        {
            Guard.StringNotNullOrEmpty(() => name);
            Id = id;
            Name = name;
        }

        public SpielerId Id { get; }
        public string Name { get; }
    }
}