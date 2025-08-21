using JassApp.Common.InformationHandling;
using JassApp.Common.LanguageExtensions.Types.Eithers;
using JassApp.Domain.Coiffeur.Models;

namespace JassApp.Domain.Coiffeur.Repositories
{
    public interface ICoiffeurSpielrundeRepository
    {
        Task DeleteAsync(CoiffeurSpielrundeId rundeId);
        Task<IReadOnlyCollection<CoiffeurSpielrunde>> LoadAllAsync();
        Task<CoiffeurSpielrunde> LoadAsync(CoiffeurSpielrundeId rundeId);
        Task<Either<InformationEntries, int>> SaveAsync(CoiffeurSpielrunde runde);
    }
}