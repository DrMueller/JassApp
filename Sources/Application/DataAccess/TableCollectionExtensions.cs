using JassApp.Common.Extensions;
using JassApp.DataAccess.Tables.Base;

namespace JassApp.DataAccess
{
    // These are used for tracked entitiy collections, therefore keep them in this layer
    internal static class TableCollectionExtensions
    {
        public static T SingleOrAdd<T>(this ICollection<T> col, Predicate<T> match)
            where T : TableBase, new()
        {
            var existingMatch = col.SingleOrDefault(f => match(f));

            if (existingMatch is not null)
            {
                return existingMatch;
            }

            var newItem = new T();
            col.Add(newItem);
            return newItem;
        }

        internal static void RemoveDeletedEntries<TTable, TModel>(
            this ICollection<TTable> entities,
            IReadOnlyCollection<TModel> models,
            Func<TModel, int> keySelector)
            where TTable : TableBase
        {
            var existingIds = entities.Select(f => f.Id).ToList();

            var modelKeys = models.Select(keySelector).ToList();
            var idsToDelete = existingIds.Except(modelKeys).ToList();
            entities.RemoveAll(f => idsToDelete.Contains(f.Id));
        }

        internal static TEntity SingleOrAdd<TEntity>(
            this ICollection<TEntity> col, int id)
            where TEntity : TableBase, new()
        {
            if (id != 0)
            {
                var existing = col.FirstOrDefault(e => e.Id == id);
                if (existing != null)
                {
                    return existing;
                }
            }

            var created = new TEntity();
            col.Add(created);
            return created;
        }
    }
}