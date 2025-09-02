using JassApp.DataAccess.Tables.Base;

namespace JassApp.DataAccess.Extensions
{
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

        public static T SingleOrAddById<T>(this ICollection<T> col, int id)
            where T : TableBase, new()
        {
            if (id == 0)
            {
                var newItem = new T();
                col.Add(newItem);
                return newItem;
            }

            var existingMatch = col.SingleOrDefault(f => f.Id == id);

            if (existingMatch is not null)
            {
                return existingMatch;
            }

            var newItem2 = new T();
            col.Add(newItem2);
            return newItem2;
        }
    }
}