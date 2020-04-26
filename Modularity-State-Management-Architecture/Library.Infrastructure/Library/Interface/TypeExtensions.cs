using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
namespace Library.Infrastructure.Library.Interface
{
    public static class TypeExtensions
    {
        //Do not take word "primitive" literally here - it is merely used to signify everything that does not need sub-textboxes
        public static bool IsPrimitive(this Type t)
        {
            bool isPrimitive = false;

            if (IsSimplePrimitive(t) || IsSimpleCSType(t) || IsGenericList(t))
            {
                isPrimitive = true;
            }
            else if (IsGenericCSType(t) || IsNullable(t))
            {
                if (IsSimplePrimitive(GetInnerGenericType(t)))
                {
                    isPrimitive = true;
                }
            }

            return isPrimitive;
        }

        public static Type GetInnerGenericType(this Type t)
        {
            Type[] types = t.GetGenericArguments();

            if (types.Length == 1)
            {
                return types[0];
            }
            else
            {
                throw new Exception("The generic type does not have 1 generic argument, as expected.");
            }
        }
        public static bool IsSimpleCSType(this Type t)
        {
            return t.Equals(typeof(DateTime?)) ||
                   t.Equals(typeof(bool?)) /*||
                   t.Equals(typeof(CSMoney))*/ ||
                   IsNumericCSType(t);
        }
        public static bool IsNumericCSType(this Type t)
        {
            return t.Equals(typeof(long?)) ||
                   t.Equals(typeof(int?)) ||
                   t.Equals(typeof(double?)) ||
                   t.Equals(typeof(short?));
        }
        public static bool IsGenericCSType(this Type t)
        {
            return IsGenericSomething(t, typeof(CSGenericType<>));
        }
        public static bool IsNullable(this Type t)
        {
            return IsGenericSomething(t, typeof(Nullable<>));
        }
        public static bool IsNumber(this Type t)
        {
            return
                typeof(short) == t || typeof(short?) == t ||
                IsInt(t) ||
                IsLong(t) ||
                IsFloatingPoint(t);
        }
        public static bool IsInt(this Type t)
        {
            return typeof(int) == t || typeof(int?) == t;
        }
        public static bool IsLong(this Type t)
        {
            return typeof(long) == t || typeof(long?) == t;
        }
        public static bool IsFloatingPoint(this Type t)
        {
            return IsDouble(t) || IsDecimal(t);
        }
        public static bool IsDouble(this Type t)
        {
            return typeof(double) == t || typeof(double?) == t;
        }
        public static bool IsDecimal(this Type t)
        {
            return typeof(decimal) == t || typeof(decimal?) == t;
        }

        public static bool IsGenericList(this Type t)
        {
            return IsGenericSomething(t, typeof(List<>));
        }
        public static bool IsSomeList(this Type t)
        {
            return (typeof(IList).IsAssignableFrom(t) || IsGenericSomething(t, typeof(IList<>))) && t != typeof(byte[]);
        }

        public static bool IsEnumerable(this Type t)
        {
            return typeof(IEnumerable).IsAssignableFrom(t) && t != typeof(string);
        }

        public static bool IsGenericSomething(this Type whatToCheck, Type TSomething)
        {
            bool isSomething = false;

            if (whatToCheck.IsGenericType)
            {
                if (whatToCheck.GetGenericTypeDefinition().Equals(TSomething))
                {
                    isSomething = true;
                }
            }

            return isSomething;
        }
        private static bool IsSimplePrimitive(Type t)
        {
            return t.IsPrimitive ||
                t.IsEnum ||
                t.Equals(typeof(string)) ||
                t.Equals(typeof(DateTime)) ||
                (typeof(Delegate).IsAssignableFrom(t)) ||
                (typeof(MethodBase).IsAssignableFrom(t)) ||
                t.Equals(typeof(object));
        }

        public static bool IsA<T>(this Type t)
        {
            return typeof(T).IsAssignableFrom(t);
        }


        /// <summary>
        /// Please note: unfortunately Enum.Parse() method cannot handle int versions of orred enums unless
        /// the enum is marked with FlagsAttribute - so make sure to put this attribute on the enum before
        /// calling this method
        /// </summary>
        /// <param name="allPossibleEnumCombinations"></param>
        /// <param name="curValueToParse"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<object> GetSplitEnumValues(this Type enumType, string curValueToParse)
        {
            List<object> splitValues = new List<object>();

            if (enumType != null && curValueToParse != null)
            {
                //a bit hacky, but should work
                object myEnumValue = Enum.Parse(enumType, curValueToParse); //both, string equivalents and int strings are handled by Parse method
                if (myEnumValue != null) //check if it parsed
                {
                    string enumValString = myEnumValue.ToString(); //have to get this reference since key could be a string containing an int
                    Array allowedValues = Enum.GetValues(enumType);
                    foreach (object o in allowedValues)
                    {
                        string tempAllowedValue = o.ToString();
                        if (enumValString.Contains(tempAllowedValue)) //TODO - need better analysis!
                        {
                            splitValues.Add(o);
                        }
                    }
                }
            }

            return splitValues;
        }

        /// <summary>
        /// WARNING: this method may be very slow, so use it with caution
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsSerializable(this Type t)
        {
            bool isSerializable = true;
            isSerializable = t.IsSerializable;

            if (isSerializable)
            {
                PropertyInfo[] props = t.GetProperties();

                foreach (PropertyInfo prop in props)
                {
                    if (!prop.PropertyType.IsSerializable)
                    {
                        isSerializable = false;
                        break;
                    }
                }
            }

            return isSerializable;
        }
        public static string GetTypeString(this Type t)
        {
            if (t == null)
            {
                return "Unknown Type";
            }

            StringBuilder sb = new StringBuilder();

            if (t.IsGenericType)
            {
                if (IsNullable(t))
                {
                    sb.Append(GetInnerGenericType(t).Name);
                    sb.Append("?");
                }
                else
                {
                    string typeName = t.GetGenericTypeDefinition().Name;
                    int end = typeName.LastIndexOf("`");
                    typeName = typeName.Substring(0, end);
                    sb.Append(typeName);
                    sb.Append("<");

                    Type[] ttypes = t.GetGenericArguments();
                    int numTypes = ttypes.Length;

                    for (int i = 0; i < numTypes; i++)
                    {
                        sb.Append(GetTypeString(ttypes[i]));

                        if (i + 1 != numTypes)
                        {
                            sb.Append(", ");
                        }
                    }
                    sb.Append(">");
                }
            }
            else
            {
                sb.Append(t.Name);
                var wrapper = t.DeclaringType;
                if (wrapper != null)
                {
                    sb.Insert(0, ".");
                    sb.Insert(0, wrapper.Name);
                }
            }

            return sb.ToString();
        }
    }
}
