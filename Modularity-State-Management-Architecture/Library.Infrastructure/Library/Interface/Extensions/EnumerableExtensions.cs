using System;
using System.Collections.Generic;
using System.Linq;

namespace StateBuilder.Library.Interface.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> target, T value)
        {
            return target.Concat(new [] { value });
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> target, T value)
        {
            return target.Except(new [] { value });
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<T> target, T value)
        {
            return target.Union(new [] { value });
        }

        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

    }
}