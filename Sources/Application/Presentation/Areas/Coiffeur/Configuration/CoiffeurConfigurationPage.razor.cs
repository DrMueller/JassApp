using JassApp.Common.InformationHandling;
using JassApp.DataAccess.Repositories;
using JassApp.Domain.Models;
using JassApp.Domain.Services;
using JassApp.Presentation.Areas.Coiffeur.RunningGame;
using JassApp.Presentation.Infrastructure.Navigation.Models;
using JassApp.Presentation.Infrastructure.Navigation.Services;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.Configuration
{
    public partial class CoiffeurConfigurationPage
    {
        public const string Path = "coiffeur/configuration";

        [Inject]
        public required INavigator Navigator { get; set; }

        [Inject]
        public required ICoiffeurSpielrundeFactory RundeFactory { get; set; }

        [Inject]
        public required ICoiffeurSpielrundeRepository RundeRepo { get; set; }

        [Inject]
        public required ISpielerRepository SpielerRepo { get; set; }

        private InformationEntries? Infos { get; set; }

        private bool IsLoading => Spieler == null;
        private int Punktwert { get; set; }

        private Domain.Models.Spieler? SelectedSpieler1 { get; set; }
        private Domain.Models.Spieler? SelectedSpieler2 { get; set; }
        private Domain.Models.Spieler? SelectedSpieler3 { get; set; }
        private Domain.Models.Spieler? SelectedSpieler4 { get; set; }

        private CoiffeurSpielrundeTyp SelectedTyp { get; set; } = CoiffeurSpielrundeTyp.WithGschobna;
        private IReadOnlyCollection<Domain.Models.Spieler>? Spieler { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Spieler = await SpielerRepo.LoadAllAsync();
        }

        private async Task StartGameAsync()
        {
            (Infos, var runde) = RundeFactory
                .TryCreating(
                    Punktwert,
                    SelectedTyp,
                    SelectedSpieler1,
                    SelectedSpieler2,
                    SelectedSpieler3,
                    SelectedSpieler4)
                .ToTuple(() => null!);

            if (Infos.HasErrorsOrWarnings)
            {
                return;
            }

            (Infos, var newId) = await RundeRepo
                .SaveAsync(runde)
                .ToTupleAsync(() => 0);

            if (Infos.HasErrorsOrWarnings)
            {
                return;
            }

            Navigator.NavigateTo
            (AppPath.Create(RunningGamePage.Path)
                .Format(newId));
        }
    }
}