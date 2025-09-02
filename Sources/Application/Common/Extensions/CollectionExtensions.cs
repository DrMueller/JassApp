namespace JassApp.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> col, IEnumerable<T> entries)
        {
            foreach (var entry in entries)
            {
                col.Add(entry);
            }
        }

        public static void RemoveAll<T>(this ICollection<T> col, Predicate<T> match)
        {
            var matches = col.Where(f => match(f)).ToList();
            foreach (var itm in matches)
            {
                col.Remove(itm);
            }
        }

        public static T SingleOrAdd<T>(this ICollection<T> col, Predicate<T> match)
            where T : new()
        {
            var existingMatch = col.SingleOrDefault(f => match(f));

            if (existingMatch != null)
            {
                return existingMatch;
            }

            var newItem = new T();
            col.Add(newItem);
            return newItem;
        }
    }
}