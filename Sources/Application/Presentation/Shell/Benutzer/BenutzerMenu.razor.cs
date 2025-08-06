using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;

namespace JassApp.Presentation.Shell.Benutzer
{
    public partial class BenutzerMenu : IDisposable
    {
        [Inject]
        public required AuthenticationStateProvider AuthStateProvider { get; set; }

        [Inject]
        public required NavigationManager Navigation { get; set; }

        private string CurrentUrl { get; set; } = string.Empty;

        private string EmailAddress { get; set; } = string.Empty;

        public void Dispose()
        {
            Navigation.LocationChanged -= OnLocationChanged;
        }

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthStateProvider.GetAuthenticationStateAsync();
            EmailAddress = auth.User.Identity!.Name!;

            CurrentUrl = Navigation.Uri;
            Navigation.LocationChanged += OnLocationChanged;
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            CurrentUrl = Navigation.Uri;
            StateHasChanged();
        }
    }
}