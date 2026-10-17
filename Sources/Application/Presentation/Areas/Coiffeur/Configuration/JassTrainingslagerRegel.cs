using SpielerModel = JassApp.Domain.Spieler.Models.Spieler;

namespace JassApp.Presentation.Areas.Coiffeur.Configuration
{
    public static class JassTrainingslagerRegel
    {
        private static readonly HashSet<string> _trainingslagerSpieler = new(StringComparer.Ordinal)
        {
            "Founi",
            "Leiser",
            "Müller",
            "Linse"
        };

        public static bool ShouldAutoSelect(IReadOnlyCollection<SpielerModel?> spieler)
        {
            if (spieler.Count != _trainingslagerSpieler.Count || spieler.Any(f => f is null))
            {
                return false;
            }

            var selectedNames = spieler.Select(f => f!.Name).ToHashSet(StringComparer.Ordinal);
            return selectedNames.SetEquals(_trainingslagerSpieler);
        }
    }
}