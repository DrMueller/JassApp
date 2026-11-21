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
using SpielerModel = JassApp.Domain.Spieler.Models.Spieler;

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

        private bool IsJasstrainingslager { get; set; }
        private bool HasRoundedPunkte { get; set; } = true;
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
                    StartSpieler,
                    IsJasstrainingslager,
                    HasRoundedPunkte)
                .ToTuple(() => null!);

            if (Infos.HasErrorsOrWarnings)
            {
                return;
            }

            using var uow = UowFactory.Create();
            var rundeRepo = uow.GetRepository<ICoiffeurSpielrundeRepository>();

            (Infos, var rundeTable) = await rundeRepo
                .SaveAsync(runde)
                .ToNullableTupleAsync(() => null);

            if (Infos.HasErrorsOrWarnings)
            {
                return;
            }

            await uow.CommitAsync();

            Navigator.NavigateTo
            (AppPath.Create(RunningGamePage.Path)
                .Format(rundeTable!.Id, false));
        }

        private void HandleSpieler1Selected(SpielerModel spieler)
        {
            SelectedSpieler1 = spieler;
            UpdateIsJasstrainingslager();
        }

        private void HandleSpieler2Selected(SpielerModel spieler)
        {
            SelectedSpieler2 = spieler;
            UpdateIsJasstrainingslager();
        }

        private void HandleSpieler3Selected(SpielerModel spieler)
        {
            SelectedSpieler3 = spieler;
            UpdateIsJasstrainingslager();
        }

        private void HandleSpieler4Selected(SpielerModel spieler)
        {
            SelectedSpieler4 = spieler;
            UpdateIsJasstrainingslager();
        }

        private void UpdateIsJasstrainingslager()
        {
            IsJasstrainingslager = JassTrainingslagerRegel.ShouldAutoSelect(
                [SelectedSpieler1, SelectedSpieler2, SelectedSpieler3, SelectedSpieler4]);
        }
    }
}
