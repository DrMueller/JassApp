using JassApp.Domain.Jassrunden.Models;
using JassApp.Domain.Jassrunden.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.JassStatistiken
{
    [UsedImplicitly]
    public partial class JassSpielSimulatorPage
    {
        public const string Path = "/jass/simulator";

        [Inject]
        public required IJassSpielrundeFactory RundeFactory { get; set; }

        private bool IsLoading => Runde == null;

        private JassSpielrunde? Runde { get; set; }

        protected override Task OnInitializedAsync()
        {
            Runde = RundeFactory.Create();

            return Task.CompletedTask;
        }

        private void CreateNeueRunde()
        {
            Runde = RundeFactory.Create();
        }
    }
}