using System.Collections.Generic;

namespace StateBuilder.Library.Interface.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary[key];
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.TryGetValue(key, out var value) ? value : default(TValue);
        }

        public static TValue GetValue<TValue>(this IDictionary<string, object> dictionary, string key)
        {
            return (TValue)dictionary[key];
        }

        public static TValue GetValueOrDefault<TValue>(this IDictionary<string, object> dictionary, string key)
        {
            return dictionary.TryGetValue(key, out var value) ? (TValue)value : default(TValue);
        }
    }
}