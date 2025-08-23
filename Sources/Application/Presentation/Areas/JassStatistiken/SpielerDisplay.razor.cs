using JassApp.Domain.Jassrunden.Models;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Areas.JassStatistiken
{
    public partial class SpielerDisplay
    {
        [Parameter]
        [EditorRequired]
        public required SpielerOrientation Orientation { get; set; }

        [Parameter]
        [EditorRequired]
        public required JasshandSpieler Spieler { get; set; }

        private string HandCssClass
        {
            get
            {
                if (Orientation == SpielerOrientation.Top || Orientation == SpielerOrientation.Bottom)
                {
                    return "hand";
                }

                return "hand vertical";
            }
        }

        private string SeatCssClass
        {
            get
            {
                return Orientation switch
                {
                    SpielerOrientation.Top => "top",
                    SpielerOrientation.Right => "right",
                    SpielerOrientation.Bottom => "bottom",
                    SpielerOrientation.Left => "left",
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }
}