using JassApp.Domain.Jassrunden.Models;
using JassApp.Domain.Jassrunden.Models.Jass;

namespace JassApp.Domain.Jassrunden.Services
{
    public interface IJassSpielrundeFactory
    {
        JassSpielrunde Create();
    }
}