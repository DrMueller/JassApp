namespace JassApp.Domain.Jassrunden.Services.Servants
{
    public static class RandomSingleton
    {
        private static Lazy<Random> _lazyRandom = new Lazy<Random>(() => new Random());
        public static Random Instance => _lazyRandom.Value;
    }
}