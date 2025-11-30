using JassApp.Common.Settings.Provisioning.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace JassApp.Presentation.Shell.Benutzer
{
    public partial class BenutzerMenu
    {
        private const string GitHubCommitPath = "https://github.com/DrMueller/JassApp/commit/{0}";

        [Inject]
        public required AuthenticationStateProvider AuthStateProvider { get; set; }

        [Inject]
        public required ISettingsProvider SettingsProvider { get; set; }

        private string AppVersion { get; set; } = string.Empty;
        private string CommitLink { get; set; } = string.Empty;
        private string EmailAddress { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            AppVersion = SettingsProvider.AppSettings.AppVersion;
            CommitLink = string.Format(GitHubCommitPath, SettingsProvider.AppSettings.GitHubCommit);

            var auth = await AuthStateProvider.GetAuthenticationStateAsync();
            EmailAddress = auth.User.Identity!.Name!;
        }
    }
}