using JassApp.DataAccess.Repositories;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.Configuration
{
    public partial class CoiffeurConfigurationPage
    {
        public const string Path = "coiffeur/configuration";

        [Inject]
        public required ISpielerRepository SpielerRepo { get; set; }

        private bool IsLoading => Spieler == null;
        private IReadOnlyCollection<Domain.Models.Spieler>? Spieler { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Spieler = await SpielerRepo.LoadAllAsync();
        }
    }
}