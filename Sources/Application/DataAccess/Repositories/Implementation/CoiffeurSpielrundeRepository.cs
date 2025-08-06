using JassApp.Common.InformationHandling;
using JassApp.Common.LanguageExtensions.Types.Eithers;
using JassApp.Domain.Models;
using JetBrains.Annotations;

namespace JassApp.DataAccess.Repositories.Implementation
{
    [UsedImplicitly]
    public class CoiffeurSpielrundeRepository : ICoiffeurSpielrundeRepository
    {
        private static readonly List<CoiffeurSpielrunde> _runden = new();

        public Task<CoiffeurSpielrunde> LoadAsync(CoiffeurSpielrundeId rundeId)
        {
            var runde = _runden.FirstOrDefault(r => r.Id == rundeId);
            if (runde == null)
            {
                throw new KeyNotFoundException($"Runde mit ID {rundeId} nicht gefunden.");
            }

            return Task.FromResult(runde);
        }

        public Task<Either<InformationEntries, int>> SaveAsync(CoiffeurSpielrunde runde)
        {
            _runden.Add(runde);
            return Task.FromResult<Either<InformationEntries, int>>(InformationEntries.Empty);
        }
    }
}