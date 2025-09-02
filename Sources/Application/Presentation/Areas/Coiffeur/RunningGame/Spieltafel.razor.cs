using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Coiffeur.Repositories;
using JassApp.Domain.Shared.Data.Writing;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class Spieltafel
    {
        private CancellationTokenSource? _cts;

        [Inject]
        public required IUnitOfWorkFactory UowFactory { get; set; }

        [Parameter]
        [EditorRequired]
        public required CoiffeurSpielrunde Spielrunde { get; set; }

        private async Task HandleValueChangedAsync()
        {
            if (_cts != null)
            {
                await _cts.CancelAsync();
            }

            _cts = new CancellationTokenSource();

            try
            {
                await Task.Delay(3000, _cts.Token);

                using var uow = UowFactory.Create();
                var repo = uow.GetRepository<ICoiffeurSpielrundeRepository>();
                await repo.SaveAsync(Spielrunde);
                await uow.CommitAsync();
            }
            catch (TaskCanceledException)
            {
            }

            StateHasChanged();
        }
    }
}