using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Skender.Stock.Indicators
{

    // GENERIC TRANSFORMS

    public static class Transforms
    {
        // TO COLLECTION
        internal static Collection<T> ToCollection<T>(this IEnumerable<T> source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var collection = new Collection<T>();
            foreach (T item in source) collection.Add(item);

            return collection;
        }
    }
}
