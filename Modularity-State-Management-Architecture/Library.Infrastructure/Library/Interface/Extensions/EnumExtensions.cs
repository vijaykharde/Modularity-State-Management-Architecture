using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace StateBuilder.Library.Interface.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString()).SingleOrDefault()?.GetCustomAttribute<DescriptionAttribute>()?.Description;
        }

        public static T? ConvertToNullableEnum<T>(this Enum someEnumVal)
            where T : struct
        {
            return someEnumVal?.ToString().ParseToEnum<T>();
        }

        public static T ConvertToEnum<T>(this Enum someEnumVal)
            where T : struct
        {

            var result = someEnumVal.ToString().ParseToEnum<T>();
            if (!result.HasValue)
            {
                throw new ArgumentNullException(nameof(T));
            }
            return result.Value;
        }
    }
}
