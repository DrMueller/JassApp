using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Coiffeur.Repositories;
using JassApp.Domain.Coiffeur.Specifications;
using JassApp.Domain.Shared.Data.Querying;
using JassApp.Domain.Shared.Data.Writing;
using JassApp.Presentation.Infrastructure.Timers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class Spieltafel : IAsyncDisposable
    {
        private bool _isDirty;
        private RunningTask? _runningTask;

        [Inject]
        public required IQueryService QueryService { get; set; }

        [Parameter]
        [EditorRequired]
        public required bool SpectatorMode { get; set; }

        [Parameter]
        [EditorRequired]
        public required CoiffeurSpielrunde Spielrunde { get; set; }

        [Inject]
        public required IUnitOfWorkFactory UowFactory { get; set; }

        private Color DirtyColor => _isDirty ? Color.Error : Color.Success;

        public async ValueTask DisposeAsync()
        {
            if (_runningTask is not null)
            {
                await _runningTask.DisposeAsync();
            }
        }

        protected override void OnInitialized()
        {
            var ts = TimeSpan.FromSeconds(5);
            _runningTask = TimerRunner.Run(AlignDataAsync, ts);
        }

        private async Task AlignDataAsync()
        {
            if (!SpectatorMode)
            {
                if (!_isDirty)
                {
                    return;
                }

                using var uow = UowFactory.Create();
                var repo = uow.GetRepository<ICoiffeurSpielrundeRepository>();
                await repo.SaveAsync(Spielrunde);
                await uow.CommitAsync();
                _isDirty = false;
                StateHasChanged();
            }
            else
            {
                Spielrunde = await QueryService.QuerySingleAsync(new CoiffeurSpielrundeSpec(Spielrunde.Id));
                StateHasChanged();
            }
        }

        private void HandleValueChanged()
        {
            _isDirty = true;
            StateHasChanged();
        }
    }
}