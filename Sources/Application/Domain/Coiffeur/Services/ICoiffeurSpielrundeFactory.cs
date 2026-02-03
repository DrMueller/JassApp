using JassApp.Common.InformationHandling;
using JassApp.Common.LanguageExtensions.Types.Eithers;
using JassApp.Domain.Coiffeur.Models;

namespace JassApp.Domain.Coiffeur.Services
{
    public interface ICoiffeurSpielrundeFactory
    {
        Either<InformationEntries, CoiffeurSpielrunde> TryCreating(
            int punkteWert,
            CoiffeurSpielrundeTyp typ,
            Spieler.Models.Spieler? spieler1,
            Spieler.Models.Spieler? spieler2,
            Spieler.Models.Spieler? spieler3,
            Spieler.Models.Spieler? spieler4,
            Spieler.Models.Spieler? startSpieler,
            bool includeRaucherpausen,
            bool includeShots
        );
    }
}