using JassApp.Common.LanguageExtensions.Invariance;

namespace JassApp.Domain.Models
{
    public record Spieler
    {
        public Spieler(
            int id,
            string name)
        {
            Guard.StringNotNullOrEmpty(() => name);
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}