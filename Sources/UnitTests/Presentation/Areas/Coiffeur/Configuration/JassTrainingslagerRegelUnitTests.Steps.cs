using JassApp.UnitTests.TestingInfrastructure.DomainModelBuilders;
using SpielerId = JassApp.Domain.Spieler.Models.SpielerId;
using SpielerModel = JassApp.Domain.Spieler.Models.Spieler;

namespace JassApp.UnitTests.Presentation.Areas.Coiffeur.Configuration
{
    public partial class JassTrainingslagerRegelUnitTests
    {
        private static(SpielerModel Founi, SpielerModel Leiser, SpielerModel Mueller, SpielerModel Linse) CreateJassTrainingslagerSpieler()
        {
            var (founi, leiser, mueller, linse) = SpielerTestBuilder.Create();
            founi.UpdateName("Founi");
            leiser.UpdateName("Leiser");
            mueller.UpdateName("Müller");
            linse.UpdateName("Linse");

            return (founi, leiser, mueller, linse);
        }

        private static SpielerModel CreateOtherSpieler()
        {
            const int otherSpielerId = 5;
            const string otherSpielerName = "Anderer Spieler";

            return new SpielerModel(new SpielerId(otherSpielerId), otherSpielerName, []);
        }
    }
}