using JassApp.Common.Extensions;
using JassApp.Common.InformationHandling;
using JassApp.Domain.Shared.Data.Querying;
using JassApp.Domain.Shared.Data.Writing;
using JassApp.Domain.Spieler.Models;
using JassApp.Domain.Spieler.Services;
using JassApp.Domain.Spieler.Specifications;
using JassApp.Presentation.Infrastructure.Navigation.Services;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Spieler
{
    public partial class SpielerEditPage
    {
        public const string Path = "/players/edit/{playerId:int}";

        public SpielerEditViewModel? EditModel { get; set; }

        [Inject]
        public required INavigator Navigator { get; set; }

        [Parameter]
        public int PlayerId { get; set; }

        [Inject]
        public required IQueryService QueryService { get; set; }

        [Inject]
        public required IUnitOfWorkFactory UowFactory { get; set; }

        private InformationEntries? Infos { get; set; }
        private bool IsLoading => EditModel == null;

        protected override async Task OnInitializedAsync()
        {
            if (PlayerId == 0)
            {
                EditModel = new SpielerEditViewModel
                {
                    Id = 0
                };

                return;
            }

            EditModel = await QueryService
                .QuerySingleAsync(new SpielerSpec(new SpielerId(PlayerId)))
                .MapAsync(s => new SpielerEditViewModel
                {
                    Id = s.Id.Value,
                    Name = s.Name
                });
        }

        private void Cancel()
        {
            Navigator.NavigateTo(SpielerOverviewPage.Path);
        }

        private async Task SaveAsync()
        {
            var spieler = EditModel!.Id == 0
                ? new Domain.Spieler.Models.Spieler(
                    new SpielerId(EditModel.Id),
                    EditModel.Name!,
                    [])
                : await QueryService.QuerySingleAsync(new SpielerSpec(new SpielerId(EditModel.Id)));

            spieler.UpdateName(EditModel.Name!);

            using var uow = UowFactory.Create();
            var spielerRepo = uow.GetRepository<ISpielerRepository>();
            Infos = await spielerRepo.SaveAsync(spieler);

            if (Infos.IsEmpty)
            {
                await uow.CommitAsync();
                Navigator.NavigateTo(SpielerOverviewPage.Path);
            }
        }
    }
}