using JassApp.Common.InformationHandling;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Shared.Infos
{
    public partial class Informations
    {
        [Parameter]
        public string? DataTestId { get; set; }

        [Parameter]
        public InformationEntries? Entries { get; set; }

        private bool HasErrors => Entries?.HasErrors ?? false;

        private bool HasInfos => Entries?.InfoMessages.Any() ?? false;

        private bool HasWarnings => Entries?.WarningMessages.Any() ?? false;
    }
}