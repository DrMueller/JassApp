using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Coiffeur.Repositories;
using JassApp.Domain.Coiffeur.Specifications;
using JassApp.Domain.Shared.Data.Querying;
using JassApp.Domain.Shared.Data.Writing;
using JassApp.Presentation.Areas.Coiffeur.RunningGame;
using JassApp.Presentation.Infrastructure.Navigation.Models;
using JassApp.Presentation.Infrastructure.Navigation.Services;
using JassApp.Presentation.Shared.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JassApp.Presentation.Areas.Coiffeur.Overview
{
    public partial class CoiffeurOverviewList
    {
        [Inject]
        public required IDialogService DialogService { get; set; }

        [Inject]
        public required INavigator Navigator { get; set; }

        [Inject]
        public required IQueryService QueryService { get; set; }

        [Inject]
        public required IUnitOfWorkFactory UowFactory { get; set; }

        private bool IsLoading => Runden == null;
        private IReadOnlyCollection<CoiffeurSpielrunde>? Runden { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task DeleteRundeAsync(int rundeId)
        {
            var dialogReference = await DialogService.ShowAsync<DeleteDialog>("Löschen");
            var result = await dialogReference.Result;
            if (result!.Canceled)
            {
                return;
            }

            using var uow = UowFactory.Create();
            var repo = uow.GetRepository<ICoiffeurSpielrundeRepository>();
            await repo.DeleteAsync(new CoiffeurSpielrundeId(rundeId));
            await uow.CommitAsync();

            await LoadAsync();
        }

        private void EditRunde(int rundeId)
        {
            Navigator.NavigateTo(AppPath.Create(RunningGamePage.Path).Format(rundeId), true);
        }

        private async Task LoadAsync()
        {
            Runden = await QueryService.QueryAsync(new CoiffeurSpielrundeSpec());
        }
    }
}