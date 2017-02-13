using System.Collections.Generic;

namespace MyMoods.Util
{
    public static class DictionaryExtensions
    {
        public static void AddOrAppend<TKey, TValue>(this Dictionary<TKey, ICollection<TValue>> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Add(value);
            }

            dictionary.Add(key, new List<TValue>() { value });
        }
    }
}
