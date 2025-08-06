using JassApp.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class TrumpfDetails
    {
        [Parameter]
        [EditorRequired]
        public required Trumpfrunde Trumpfrunde { get; set; }
    }
}
