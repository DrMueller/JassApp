using JassApp.Common.LanguageExtensions.Types.Maybes;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Presentation.Infrastructure.Timers;
using JassApp.Presentation.Shared.Voices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class RunningGameInfos : IAsyncDisposable
    {
        private RunningTask? _runningTask;
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
            if (_runningTask is not null)
            {
                await _runningTask.DisposeAsync();
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                var ts = TimeSpan.FromSeconds(5);
                _runningTask = TimerRunner.Run(CheckDataAsync, ts);
            }
        }

        private async Task CheckDataAsync()
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

        private async Task InformAsync(string info)
        {
            Snackbar.Add(info, Severity.Success);
            await VoiceRef.SpeakAsync(info);
        }
    }
}