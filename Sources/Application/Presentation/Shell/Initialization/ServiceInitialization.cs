using JassApp.Common.Settings.Provisioning.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using MudBlazor.Services;

namespace JassApp.Presentation.Shell.Initialization
{
    public static class ServiceInitialization
    {
        public static void Initialize(IServiceCollection services)
        {
            services.AddMudServices();
            services.Configure<AppSettings>(Program.Configuration.GetSection(AppSettings.SectionKey));

            services.AddRazorComponents()
                .AddInteractiveServerComponents();

            services.AddCors();
            services.AddAntiforgery();

            services.AddHsts(options =>
            {
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(730);
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = _ => true;
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            });

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Program.Configuration.GetSection("AzureAd"));

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = options.DefaultPolicy;
            });
        }
    }
}