using System;

namespace StateBuilder.Library.Interface.Extensions
{
    public static class StringExtensions
    {
        public static T? ParseToEnum<T>(this string searchString) where T : struct
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return null;
            }

            if (!typeof(T).IsEnum)
            {
                return null;
            }

            var enumValues = Enum.GetNames(typeof(T));
            foreach (var s in enumValues)
            {
                if (searchString.Equals(s, StringComparison.InvariantCultureIgnoreCase))
                {
                    return (T)Enum.Parse(typeof(T), searchString, true);
                }
            }

            return null;
        }
    }
}
