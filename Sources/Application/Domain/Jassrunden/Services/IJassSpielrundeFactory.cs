using JassApp.Domain.Jassrunden.Models;

namespace JassApp.Domain.Jassrunden.Services
{
    public interface IJassSpielrundeFactory
    {
        JassSpielrunde Create();
    }
}