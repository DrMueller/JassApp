using JassApp.Common.InformationHandling;
using JassApp.Domain.Shared.Data.Querying;
using JassApp.Domain.Shared.Data.Writing;
using JassApp.Domain.Spieler.Services;
using JassApp.Domain.Spieler.Specifications;
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
        public required IQueryService QueryService { get; set; }

        [Inject]
        public required IUnitOfWorkFactory UowFactory { get; set; }

        private InformationEntries? Infos { get; set; }

        private bool IsLoading => Spieler == null;
        private IReadOnlyCollection<Domain.Spieler.Models.Spieler>? Spieler { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task DeleteSpielerAsync(int spielerId)
        {
            using var uow = UowFactory.Create();
            var spielerRepo = uow.GetRepository<ISpielerRepository>();
            Infos = await spielerRepo.DeleteAsync(spielerId);
            if (Infos.IsEmpty)
            {
                await uow.CommitAsync();
                await LoadAsync();
            }
        }

        private void EditSpieler(int spielerId)
        {
            Navigator.NavigateTo(AppPath.Create(SpielerEditPage.Path).Format(spielerId), true);
        }

        private async Task LoadAsync()
        {
            Spieler = await QueryService.QueryAsync(new SpielerSpec());
        }
    }
}