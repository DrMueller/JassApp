﻿using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.Coiffeur.RunningGame
{
    public partial class TeamDetails
    {
        [Parameter]
        [EditorRequired]
        public required string TeamDescription { get; set; }
    }
}