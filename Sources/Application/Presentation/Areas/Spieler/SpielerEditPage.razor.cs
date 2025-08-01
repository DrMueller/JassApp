using JassApp.Common.Extensions;
using JassApp.Common.InformationHandling;
using JassApp.DataAccess.Repositories;
using JassApp.Presentation.Infrastructure.Navigation.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

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
        public required ISpielerRepository SpielerRepo { get; set; }

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

            EditModel = await SpielerRepo
                .LoadAsync(PlayerId)
                .MapAsync(s => new SpielerEditViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                });
        }

        private async Task HandleValidSubmitAsync(EditContext arg)
        {
            var spieler = new Domain.Models.Spieler(EditModel!.Id, EditModel.Name!);
            Infos = await SpielerRepo.SaveAsync(spieler);

            if (Infos.IsEmpty)
            {
                Navigator.NavigateTo(SpielerOverviewPage.Path);
            }
        }
    }
}