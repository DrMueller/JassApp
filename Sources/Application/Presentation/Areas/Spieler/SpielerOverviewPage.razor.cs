using JassApp.Presentation.Infrastructure.Navigation.Models;
using JassApp.Presentation.Infrastructure.Navigation.Services;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Spieler
{
    public partial class SpielerOverviewPage
    {
        public const string Path = "/players/overview";

        [Inject]
        public required INavigator Navigator { get; set; }

        private void CreateNewSpieler()
        {
            Navigator.NavigateTo(AppPath.Create(SpielerEditPage.Path).Format(0), true);
        }
    }
}