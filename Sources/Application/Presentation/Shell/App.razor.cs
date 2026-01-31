using JassApp.Presentation.Infrastructure.Caching.Controllers;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Shell
{
    public partial class App
    {
        [Inject]
        public required ICachingService CachingService { get; set; }

        private string? CacheVersionParam { get; set; }

        private bool IsLoading => string.IsNullOrEmpty(CacheVersionParam);

        protected override async Task OnInitializedAsync()
        {
            CacheVersionParam = await CachingService.LoadCachingSuffixAsync();
        }
    }
}