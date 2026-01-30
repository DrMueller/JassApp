using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Coiffeur.Repositories;
using JassApp.Domain.Shared.Data.Writing;
using JassApp.Presentation.Shared.Voice;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class Spieltafel : IAsyncDisposable
    {
        private readonly CancellationTokenSource _cts = new();

        private bool _isDirty;
        private Task? _loopTask;

        [Parameter]
        [EditorRequired]
        public required CoiffeurSpielrunde Spielrunde { get; set; }

        [Inject]
        public required IUnitOfWorkFactory UowFactory { get; set; }

        private Color DirtyColor => _isDirty ? Color.Error : Color.Success;
        private Voice VoiceRef { get; set; } = null!;

        public async ValueTask DisposeAsync()
        {
            await _cts.CancelAsync();
            if (_loopTask is not null)
            {
#pragma warning disable VSTHRD003
                await _loopTask;
#pragma warning restore VSTHRD003
            }

            _cts.Dispose();
        }

        protected override void OnInitialized()
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(5)); // interval
            _loopTask = SaveAsync(timer, _cts.Token);
        }

        private void HandleValueChanged()
        {
            _isDirty = true;
            StateHasChanged();
        }

        private async Task SaveAsync(PeriodicTimer timer, CancellationToken ct)
        {
            try
            {
                while (await timer.WaitForNextTickAsync(ct))
                {
                    while (await timer.WaitForNextTickAsync(ct))
                    {
                        using var uow = UowFactory.Create();
                        var repo = uow.GetRepository<ICoiffeurSpielrundeRepository>();
                        await repo.SaveAsync(Spielrunde);
                        await uow.CommitAsync();
                        _isDirty = false;
                        StateHasChanged();
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                timer.Dispose();
            }
        }

        private async Task SayOpenTruempfeAsync(JassTeamTyp team)
        {
            var offeneTruempfe = Spielrunde.GetOffeneTruempfe(team);

            var str = string.Join(", ", offeneTruempfe.Select(t => t.ToString()));

            await VoiceRef.PrimeAsync();
            await VoiceRef.SpeakAsync(str);
        }
    }
}