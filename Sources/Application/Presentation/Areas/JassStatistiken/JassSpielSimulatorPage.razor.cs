using JassApp.Common.InformationHandling;
using JassApp.Domain.Jassrunden.Models;
using JassApp.Domain.Jassrunden.Models.Jass;
using JassApp.Domain.Jassrunden.Services;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.JassStatistiken
{
    public partial class JassSpielSimulatorPage
    {
        public const string Path = "/jass/simulator";

        [Inject]
        public required IJassSpielrundeFactory RundeFactory { get; set; }

        private JassSpielrunde? Runde { get; set; }

        private InformationEntries? Infos { get; set; }

        private bool IsLoading => Runde == null;

        protected override Task OnInitializedAsync()
        {
            Runde = RundeFactory.Create();

            return Task.CompletedTask;
        }
    }
}
