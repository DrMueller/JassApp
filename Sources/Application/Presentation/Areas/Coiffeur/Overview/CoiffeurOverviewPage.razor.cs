using JassApp.Presentation.Areas.Coiffeur.Configuration;
using JassApp.Presentation.Infrastructure.Navigation.Services;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.Overview
{
    public partial class CoiffeurOverviewPage
    {
        public const string Path = "coiffeur/overview";

        [Inject]
        public required INavigator Navigator { get; set; }

        private void CreateNewCoiffeurPartie()
        {
            Navigator.NavigateTo(CoiffeurConfigurationPage.Path);
        }
    }
}
