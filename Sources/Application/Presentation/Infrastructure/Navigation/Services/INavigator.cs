namespace JassApp.Presentation.Infrastructure.Navigation.Services
{
    public interface INavigator
    {
        string BaseUri { get; }

        string Uri { get; }

        void NavigateTo(string target, bool forceLoad = false);

        Task OpenInNewTabAsync(string target);

        Uri ToAbsoluteUri(string? relativeUri);
    }
}