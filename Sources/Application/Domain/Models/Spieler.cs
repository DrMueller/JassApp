using JassApp.Common.LanguageExtensions.Invariance;

namespace JassApp.Domain.Models
{
    public record SpielerId(int Value);

    public class Spieler
    {
        public Spieler(
            SpielerId id,
            string name,
            IReadOnlyCollection<JassTeamÎd> assignedTeams)
        {
            Guard.StringNotNullOrEmpty(() => name);
            Id = id;
            Name = name;
            AssignedTeams = assignedTeams;
        }

        public bool CanBeDeleted => !AssignedTeams.Any();

        public SpielerId Id { get; }
        public string Name { get; private set; }
        private IReadOnlyCollection<JassTeamÎd> AssignedTeams { get; }

        public void UpdateName(string name)
        {
            Guard.StringNotNullOrEmpty(() => name);
            Name = name;
        }
    }
}