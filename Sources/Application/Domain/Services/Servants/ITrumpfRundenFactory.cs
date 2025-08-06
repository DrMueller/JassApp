using JassApp.Domain.Models;

namespace JassApp.Domain.Services.Servants
{
    public interface ITrumpfRundenFactory
    {
        IReadOnlyCollection<Trumpfrunde> Create(CoiffeurSpielrundeTyp typ);
    }
}
