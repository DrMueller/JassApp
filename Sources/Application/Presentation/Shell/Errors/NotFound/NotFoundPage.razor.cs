using JassApp.Presentation.Areas.Home;
using JassApp.Presentation.Infrastructure.Navigation.Services;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Shell.Errors.NotFound
{
    public partial class NotFoundPage
    {
        public const string Path = "/notfound";

        private static readonly string[] _excuses =
        [
            "Die Seite macht gerade Kaffee. ☕",
            "404: Route wurde von einer Katze umgeworfen. 🐈",
            "Diese URL ist auf Weltreise gegangen. 🌍",
            "Der Server hat es ausprobiert. Wirklich. 😅",
            "Ich schwöre, sie war eben noch da!",
            "Semikolon fehlt, Realität kollabiert. ;"
        ];

        private string _excuse = "Vielleicht hat ein Kobold die Route umbenannt.";

        [Inject]
        public required INavigator Navigator { get; set; }

        private void GoHome()
        {
            Navigator.NavigateTo(HomePage.Path);
        }

        private void RandomExcuse()
        {
            var i = Random.Shared.Next(_excuses.Length);
            _excuse = _excuses[i];
        }
    }
}