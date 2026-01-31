namespace JassApp.Presentation.Infrastructure.Caching.Controllers
{
    public interface ICachingService
    {
        Task<string> LoadCachingSuffixAsync();
    }
}