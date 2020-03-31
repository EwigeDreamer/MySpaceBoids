namespace MyTools.Extensions.Common
{
    using System;
    using System.Collections.Generic;
    public static class CommonEx
    {
        public static TColl GroupExecute<TColl, TObj>(this TColl collection, Action<TObj> action) 
            where TColl : IList<TObj>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (action == null) return collection;
            int count = collection.Count;
            for (int i = 0; i < count; ++i)
                action(collection[i]);
            return collection;
        }

        public static TObj FindNoBox<TColl, TObj>(this TColl collection, Predicate<TObj> match) 
            where TColl : IList<TObj>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (match == null) throw new ArgumentNullException(nameof(match));
            int count = collection.Count;
            for (int i = 0; i < count; ++i)
                if (match(collection[i])) return collection[i];
            return default;
        }

        public static void FindAllNoBox<TColl, TObj>(this TColl collection, Predicate<TObj> match, TColl otput) 
            where TColl : IList<TObj>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (otput == null) throw new ArgumentNullException(nameof(otput));
            if (match == null) throw new ArgumentNullException(nameof(match));
            otput.Clear();
            int count = collection.Count;
            for (int i = 0; i < count; ++i)
                if (match(collection[i])) otput.Add(collection[i]);
        }

        public static bool HasFieldNoBox<TColl, TObj>(this TColl collection, TObj field, out int index) 
            where TColl : IList<TObj>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (field == null) throw new ArgumentNullException(nameof(field));
            index = -1;
            int count = collection.Count;
            for (int i = 0; i < count; ++i) if (collection[i].Equals(field)) { index = i; return true; }
            return false;
        }

        public static TObj FindMaxNoBox<TColl, TObj>(this TColl collection, out int index)
            where TColl : IList<TObj>
            where TObj : IComparable<TObj>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            index = 0;
            int count = collection.Count;
            for (int i = 0; i < count; ++i)
                if (collection[i].CompareTo(collection[index]) > 0)
                    index = i;
            return collection[index];
        }

        public static TObj FindMinNoBox<TColl, TObj>(this TColl collection, out int index)
            where TColl : IList<TObj>
            where TObj : IComparable<TObj>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            index = 0;
            int count = collection.Count;
            for (int i = 0; i < count; ++i)
                if (collection[i].CompareTo(collection[index]) < 0)
                    index = i;
            return collection[index];
        }
    }
}
