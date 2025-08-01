using JassApp.Domain.Models;

namespace JassApp.Domain.Services
{
    public interface ITrumpfFactory
    {
        IReadOnlyCollection<Trumpf> CreateWithGschobna();
    }
}
