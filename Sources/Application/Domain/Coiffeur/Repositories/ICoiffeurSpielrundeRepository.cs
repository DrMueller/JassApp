using JassApp.Common.InformationHandling;
using JassApp.Common.LanguageExtensions.Types.Eithers;
using JassApp.DataAccess.Tables;
using JassApp.Domain.Coiffeur.Models;
using JassApp.Domain.Shared.Data.Writing;

namespace JassApp.Domain.Coiffeur.Repositories
{
    public interface ICoiffeurSpielrundeRepository : IRepository
    {
        Task DeleteAsync(CoiffeurSpielrundeId rundeId);
        Task<Either<InformationEntries, CoiffeurSpielrundeTable>> SaveAsync(CoiffeurSpielrunde runde);
    }
}