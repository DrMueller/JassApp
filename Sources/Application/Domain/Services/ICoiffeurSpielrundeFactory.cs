using JassApp.Common.InformationHandling;
using JassApp.Common.LanguageExtensions.Types.Eithers;
using JassApp.Domain.Models;

namespace JassApp.Domain.Services
{
    public interface ICoiffeurSpielrundeFactory
    {
        Either<InformationEntries, CoiffeurSpielrunde> TryCreating(
            int punkteWert,
            CoiffeurSpielrundeTyp typ,
            Spieler? spieler1,
            Spieler? spieler2,
            Spieler? spieler3,
            Spieler? spieler4
        );
    }
}