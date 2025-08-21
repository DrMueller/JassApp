using JassApp.Common.InformationHandling;
using JassApp.Common.LanguageExtensions.Types.Eithers;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Coiffeur.Services;
using JassApp.Domain.Coiffeur.Services.Servants;

namespace JassApp.Domain.Coiffeur.Services.Implementation
{
    public class CoiffeurSpielrundeFactory(
        ITrumpfRundenFactory trumpfrundenFactory) : ICoiffeurSpielrundeFactory
    {
        public static string SpielerNichtEindeutigErrorMessage = "Spieler müssen eindeutig sein.";

        public Either<InformationEntries, CoiffeurSpielrunde> TryCreating(
            int punkteWert,
            CoiffeurSpielrundeTyp typ,
            Spieler.Models.Spieler? spieler1,
            Spieler.Models.Spieler? spieler2,
            Spieler.Models.Spieler? spieler3,
            Spieler.Models.Spieler? spieler4,
            Spieler.Models.Spieler? startSpieler)
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

            var teamSpieler1 = new JassTeamSpieler(
                new JassTeamSpielerId(0),
                spieler1!.Id,
                spieler1.Name,
                startSpieler!.Id == spieler1.Id);

            var teamSpieler2 = new JassTeamSpieler(
                new JassTeamSpielerId(0),
                spieler2!.Id,
                spieler2.Name,
                startSpieler.Id == spieler2.Id);

            var teamSpieler3 = new JassTeamSpieler(
                new JassTeamSpielerId(0),
                spieler3!.Id,
                spieler3.Name,
                startSpieler.Id == spieler3.Id);

            var teamSpieler4 = new JassTeamSpieler(
                new JassTeamSpielerId(0),
                spieler4!.Id,
                spieler4.Name,
                startSpieler.Id == spieler4.Id);

            return new CoiffeurSpielrunde(
                new CoiffeurSpielrundeId(0),
                DateTime.Now,
                punkteWert,
                trumpfRunden,
                JassTeam.CreateNew(teamSpieler1, teamSpieler2),
                JassTeam.CreateNew(teamSpieler3, teamSpieler4));
        }

        private static InformationEntries ValidateSpieler(
            Spieler.Models.Spieler? spieler1,
            Spieler.Models.Spieler? spieler2,
            Spieler.Models.Spieler? spieler3,
            Spieler.Models.Spieler? spieler4
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