namespace JassApp.Domain.Jassrunden.Models.Jass
{
    public class JassPlayerWithJassSuiteInHand
    {
        public JassPlayer Player { get; }
        public JassSuiteInHand SuiteInHand { get; }

        public JassPlayerWithJassSuiteInHand(JassPlayer player, JassSuiteInHand suiteInHand)
        {
            Player = player;
            SuiteInHand = suiteInHand;
        }
    }
}