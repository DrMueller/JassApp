using JassApp.Common.InformationHandling;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Coiffeur.Repositories;
using JassApp.Domain.Coiffeur.Services;
using JassApp.Domain.Shared.Data.Querying;
using JassApp.Domain.Shared.Data.Writing;
using JassApp.Domain.Spieler.Specifications;
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
        public required IQueryService QueryService { get; set; }

        [Inject]
        public required ICoiffeurSpielrundeFactory RundeFactory { get; set; }

        [Inject]
        public required IUnitOfWorkFactory UowFactory { get; set; }

        private InformationEntries? Infos { get; set; }

        private bool IsLoading => Spieler == null;
        private int Punktwert { get; set; } = 20;

        private Domain.Spieler.Models.Spieler? SelectedSpieler1 { get; set; }
        private Domain.Spieler.Models.Spieler? SelectedSpieler2 { get; set; }
        private Domain.Spieler.Models.Spieler? SelectedSpieler3 { get; set; }
        private Domain.Spieler.Models.Spieler? SelectedSpieler4 { get; set; }

        private CoiffeurSpielrundeTyp SelectedTyp { get; set; } = CoiffeurSpielrundeTyp.WithGschobna;
        private IReadOnlyCollection<Domain.Spieler.Models.Spieler>? Spieler { get; set; }

        private Domain.Spieler.Models.Spieler? StartSpieler { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Spieler = await QueryService.QueryAsync(new SpielerSpec());
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
                    SelectedSpieler4,
                    StartSpieler)
                .ToTuple(() => null!);

            if (Infos.HasErrorsOrWarnings)
            {
                return;
            }

            using var uow = UowFactory.Create();
            var rundeRepo = uow.GetRepository<ICoiffeurSpielrundeRepository>();

            (Infos, var rundeTable) = await rundeRepo
                .SaveAsync(runde).ToNullableTupleAsync(() => null);

            if (Infos.HasErrorsOrWarnings)
            {
                return;
            }

            await uow.CommitAsync();

            Navigator.NavigateTo
            (AppPath.Create(RunningGamePage.Path)
                .Format(rundeTable!.Id));
        }
    }
}