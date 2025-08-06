using JassApp.Common.InformationHandling;
using JassApp.Common.LanguageExtensions.Types.Eithers;
using JassApp.Domain.Models;
using JassApp.Domain.Services.Servants;

namespace JassApp.Domain.Services.Implementation
{
    public class CoiffeurSpielrundeFactory(
        ITrumpfRundenFactory trumpfrundenFactory) : ICoiffeurSpielrundeFactory
    {
        public static string SpielerNichtEindeutigErrorMessage = "Spieler müssen eindeutig sein.";

        public Either<InformationEntries, CoiffeurSpielrunde> TryCreating(
            int punkteWert,
            CoiffeurSpielrundeTyp typ,
            Spieler? spieler1,
            Spieler? spieler2,
            Spieler? spieler3,
            Spieler? spieler4)
        {
            if (punkteWert == 0)
            {
                return InformationEntries.CreateFromError(
                    "Punktewert muss grösser als 0 sein."
                );
            }

            var spielerValidation = ValidateSpieler(spieler1, spieler2, spieler3, spieler4);
            if (spielerValidation.HasErrors)
            {
                return spielerValidation;
            }

            var trumpfRunden = trumpfrundenFactory.Create(typ);

            var teamSpieler1 = new JassTeamSpieler(spieler1!.Id, spieler1.Name);
            var teamSpieler2 = new JassTeamSpieler(spieler2!.Id, spieler2.Name);
            var teamSpieler3 = new JassTeamSpieler(spieler3!.Id, spieler3.Name);
            var teamSpieler4 = new JassTeamSpieler(spieler4!.Id, spieler4.Name);

            return new CoiffeurSpielrunde(
                new CoiffeurSpielrundeId(0),
                punkteWert,
                trumpfRunden,
                JassTeam.CreateNew(teamSpieler1, teamSpieler2),
                JassTeam.CreateNew(teamSpieler3, teamSpieler4));
        }

        private static InformationEntries ValidateSpieler(
            Spieler? spieler1,
            Spieler? spieler2,
            Spieler? spieler3,
            Spieler? spieler4
        )
        {
            if (spieler1 == null || spieler2 == null || spieler3 == null || spieler4 == null)
            {
                return InformationEntries.CreateFromError(
                    "Alle Spieler müssen gesetzt sein."
                );
            }

            if (spieler1.Id == spieler2.Id || spieler1 == spieler3 || spieler1.Id == spieler4.Id ||
                spieler2.Id == spieler3.Id || spieler2 == spieler4 || spieler3.Id == spieler4.Id)
            {
                return InformationEntries.CreateFromError(SpielerNichtEindeutigErrorMessage);
            }

            return InformationEntries.Empty;
        }
    }
}