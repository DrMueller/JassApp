using System.ComponentModel.DataAnnotations;

namespace JassApp.Presentation.Areas.Spieler
{
    public class SpielerEditViewModel
    {
        public int Id { get; init; }

        [Required]
        public string? Name { get; set; }
    }
}