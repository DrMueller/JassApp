using JassApp.Common.LanguageExtensions.Invariance;

namespace JassApp.Domain.Jassrunden.Models.Jass
{
    public class JassPlayer
    {
        public string Name { get; }
        public JassHand JassHand { get; }

        private JassPlayer _partner;

        public JassPlayer(string name)
        {
            Guard.StringNotNullOrEmpty(() => name);

            JassHand = new JassHand();
            Name = name;
        }

        public void AssignPartner(JassPlayer partner)
        {
            _partner = partner;
        }

        public bool IsPartnerOf(JassPlayer partner)
        {
            return _partner == partner;
        }
    }
}