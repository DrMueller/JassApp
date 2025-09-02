using JassApp.Domain.Coiffeur.Models;

namespace JassApp.Domain.Coiffeur.Services.Servants
{
    public interface ITrumpfRundenFactory
    {
        IReadOnlyCollection<CoiffeurTrumpfrunde> Create(CoiffeurSpielrundeTyp typ);
    }
}