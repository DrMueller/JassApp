using JassApp.Common.InformationHandling;
using JassApp.Common.LanguageExtensions.Types.Eithers;
using JassApp.Domain.Models;

namespace JassApp.DataAccess.Repositories
{
    public interface ICoiffeurSpielrundeRepository
    {
        Task<CoiffeurSpielrunde> LoadAsync(CoiffeurSpielrundeId rundeId);
        Task<Either<InformationEntries, int>> SaveAsync(CoiffeurSpielrunde runde);
    }
}