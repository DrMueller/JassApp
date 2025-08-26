using JetBrains.Annotations;

namespace JassApp.Domain.Jassrunden.Models
{
    [PublicAPI]
    public record JasskarteWert(JasskarteWertTyp JasskarteWertTyp, string Label);
}