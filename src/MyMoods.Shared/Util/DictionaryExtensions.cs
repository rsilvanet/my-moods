using System.Collections.Generic;

namespace MyMoods.Shared.Util
{
    public static class DictionaryExtensions
    {
        public static void AddOrAppend<TKey, TValue>(this Dictionary<TKey, ICollection<TValue>> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Add(value);
            }
            else
            {
                dictionary.Add(key, new List<TValue>() { value });
            }
        }
    }
}
