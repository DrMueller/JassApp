using JassApp.Common.LanguageExtensions.Types.Maybes;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Presentation.Shared.Voices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class RunningGameInfos : IAsyncDisposable
    {
        private readonly CancellationTokenSource _cts = new();
        private Task? _loopTask;
        private bool _shotsWereInformed;

        private bool _smokeWasInformed;

        [Inject]
        public required ISnackbar Snackbar { get; set; }

        [Parameter]
        [EditorRequired]
        public required CoiffeurSpielrunde Spielrunde { get; set; }

        private Voice VoiceRef { get; set; } = null!;
        private VoiceSupport VoiceSupportRef { get; set; } = null!;

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

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
                _loopTask = CheckDataAsync(timer, _cts.Token);
            }
        }

        private async Task CheckDataAsync(PeriodicTimer timer, CancellationToken ct)
        {
            try
            {
                while (await timer.WaitForNextTickAsync(ct))
                {
                    await VoiceSupportRef.CheckSupportAsync();

                    if (Spielrunde.CheckShouldSmoke() && !_smokeWasInformed)
                    {
                        _smokeWasInformed = true;
                        await InformAsync("Raucherpause");
                    }

                    await Spielrunde.CheckWhoShouldOrderShots()
                        .WhenSomeAsync(async spieler =>
                        {
                            if (_shotsWereInformed)
                            {
                                return;
                            }

                            _shotsWereInformed = true;
                            await InformAsync($"{spieler} bestellt Shots!");
                        });
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

        private async Task InformAsync(string info)
        {
            Snackbar.Add(info, Severity.Success);
            await VoiceRef.SpeakAsync(info);
        }
    }
}