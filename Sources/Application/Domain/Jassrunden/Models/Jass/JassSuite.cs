namespace JassApp.Domain.Jassrunden.Models.Jass
{
    public class JassSuite
    {
        private static Lazy<IReadOnlyCollection<JassSuite>> _allLazy = new Lazy<IReadOnlyCollection<JassSuite>>(
            () => new List<JassSuite>
            {
                new JassSuite(JassSuiteType.Ecken),
                new JassSuite(JassSuiteType.Herz),
                new JassSuite(JassSuiteType.Kreuz),
                new JassSuite(JassSuiteType.Schaufeln)
            });

        public static IReadOnlyCollection<JassSuite> All => _allLazy.Value;
        public JassSuiteType JassSuiteType { get; }

        public JassSuite(JassSuiteType jassSuiteType)
        {
            JassSuiteType = jassSuiteType;
        }

        public override string ToString()
        {
            return JassSuiteType.ToString();
        }
    }
}