using JassApp.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class TeamsDetails
    {
        [Parameter]
        [EditorRequired]
        public required JassTeam Team { get; set; }
    }
}
