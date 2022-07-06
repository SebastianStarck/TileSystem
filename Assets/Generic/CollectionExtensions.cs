using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Generic
{
    public static class CollectionExtensions
    {
        public static T[] Each<T>(this T[] enumerable, Action<T> callback)
        {
            foreach (var item in enumerable) callback(item);

            return enumerable;
        }

        public static List<T> Each<T>(this List<T> enumerable, Action<T> callback)
        {
            foreach (var item in enumerable) callback(item);

            return enumerable;
        }

        public static string JoinToString<T>(this IEnumerable<T> enumerable) => string.Join(",", enumerable);

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            var arr = enumerable.ToArray();
            Debug.Log($"Random: {UnityEngine.Random.Range(0, arr.Count() - 1)}");
            return arr[UnityEngine.Random.Range(0, arr.Count() - 1)];
        }
    }
}
