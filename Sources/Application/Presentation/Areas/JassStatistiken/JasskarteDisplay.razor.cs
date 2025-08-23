using JassApp.Domain.Jassrunden.Models;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.JassStatistiken
{
    public partial class JasskarteDisplay
    {
        [Parameter]
        [EditorRequired]
        public required Jasskarte Karte { get; set; }
    }
}