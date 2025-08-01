using JassApp.Common.InformationHandling;
using JassApp.DataAccess.Repositories;
using JassApp.Presentation.Infrastructure.Navigation.Models;
using JassApp.Presentation.Infrastructure.Navigation.Services;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Spieler
{
    public partial class SpielerOverviewList
    {
        [Inject]
        public required INavigator Navigator { get; set; }

        [Inject]
        public required ISpielerRepository SpielerRepo { get; set; }

        private InformationEntries? Infos { get; set; }

        private bool IsLoading => Spieler == null;
        private IReadOnlyCollection<Domain.Models.Spieler>? Spieler { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task DeleteSpielerAsync(int spielerId)
        {
            Infos = await SpielerRepo.DeleteAsync(spielerId);
            if (Infos.IsEmpty)
            {
                await LoadAsync();
            }
        }

        private void EditSpieler(int spielerId)
        {
            Navigator.NavigateTo(AppPath.Create(SpielerEditPage.Path).Format(spielerId), true);
        }

        private async Task LoadAsync()
        {
            Spieler = await SpielerRepo.LoadAllAsync();
        }
    }
}