using System;
using System.Linq;

namespace StateBuilder.Library.Interface.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] Concat<T>(this T[] target, T value)
        {
            return target.Concat(new [] { value }).ToArray();
        }

        public static T[] Except<T>(this T[] target, T value)
        {
            return target.Except(new [] { value }).ToArray();
        }

        public static T[] Union<T>(this T[] target, T value)
        {
            return target.Union(new [] { value }).ToArray();
        }
    }
}
