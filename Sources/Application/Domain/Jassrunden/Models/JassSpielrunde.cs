using JassApp.Common.LanguageExtensions.Invariance;

namespace JassApp.Domain.Jassrunden.Models
{
    public class JassSpielrunde
    {
        public IReadOnlyCollection<JasshandSpieler> JassPlayers { get; }

        public JassSpielrunde(IReadOnlyCollection<JasshandSpieler> jassPlayers)
        {
            Guard.ObjectNotNull(() => jassPlayers);

            if (jassPlayers.Count != 4)
            {
                throw new ArgumentException("Only exactly 4 players allowed.");
            }

            JassPlayers = jassPlayers;
        }
    }
}