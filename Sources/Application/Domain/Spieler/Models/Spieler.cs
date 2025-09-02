using JassApp.Common.LanguageExtensions.Invariance;
using JassApp.Domain.Coiffeur.Models;

namespace JassApp.Domain.Spieler.Models
{
    public record SpielerId(int Value);

    public class Spieler
    {
        public Spieler(
            SpielerId id,
            string name,
            IReadOnlyCollection<JassTeamId> assignedTeams)
        {
            Guard.StringNotNullOrEmpty(() => name);
            Id = id;
            Name = name;
            AssignedTeams = assignedTeams;
        }

        public bool CanBeDeleted => !AssignedTeams.Any();

        public SpielerId Id { get; }
        public string Name { get; private set; }
        private IReadOnlyCollection<JassTeamId> AssignedTeams { get; }

        public void UpdateName(string name)
        {
            Guard.StringNotNullOrEmpty(() => name);
            Name = name;
        }
    }
}