using JetBrains.Annotations;

namespace JassApp.Domain.Jassrunden.Models
{
    [PublicAPI]
    public record JassSuit(JassSuitType JassSuitType, string Farbe, string Symbol);
}