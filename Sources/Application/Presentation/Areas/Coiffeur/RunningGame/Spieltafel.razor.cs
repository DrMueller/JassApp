using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Coiffeur.Repositories;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class Spieltafel
    {
        private CancellationTokenSource? _cts;

        [Inject]
        public required ICoiffeurSpielrundeRepository Repo { get; set; }

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
                await Repo.SaveAsync(Spielrunde);
            }
            catch (TaskCanceledException)
            {
            }

            StateHasChanged();
        }
    }
}