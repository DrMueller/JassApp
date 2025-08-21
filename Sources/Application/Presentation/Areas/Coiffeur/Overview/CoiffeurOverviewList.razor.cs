using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Coiffeur.Repositories;
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
        public required ICoiffeurSpielrundeRepository Repo { get; set; }

        private bool IsLoading => Runden == null;
        private IReadOnlyCollection<CoiffeurSpielrunde>? Runden { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task DeleteRundeAsync(int rundeId)
        {
            var tra = await DialogService.ShowAsync<DeleteDialog>("Löschen");
            var result = await tra.Result;
            if (result!.Canceled)
            {
                return;
            }

            await Repo.DeleteAsync(new CoiffeurSpielrundeId(rundeId));
            await LoadAsync();
        }

        private void EditRunde(int rundeId)
        {
            Navigator.NavigateTo(AppPath.Create(RunningGamePage.Path).Format(rundeId), true);
        }

        private async Task LoadAsync()
        {
            Runden = await Repo.LoadAllAsync();
        }
    }
}