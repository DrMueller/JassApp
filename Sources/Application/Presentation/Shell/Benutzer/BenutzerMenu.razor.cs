using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace JassApp.Presentation.Shell.Benutzer
{
    public partial class BenutzerMenu
    {
        [Inject]
        public required AuthenticationStateProvider AuthStateProvider { get; set; }

        private string EmailAddress { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthStateProvider.GetAuthenticationStateAsync();
            EmailAddress = auth.User.Identity!.Name!;
        }
    }
}