using JassApp.Domain.Models;

namespace JassApp.Domain.Services
{
    public interface ICoiffeurSpielrundeFactory
    {
        CoiffeurSpielrunde CreateGschobna(
            int punkteWert,
            JassTeam team1,
            JassTeam team2);
    }
}
