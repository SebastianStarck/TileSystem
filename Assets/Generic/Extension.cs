using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Generic
{
    public static class MathExtensions
    {
        public static bool IsEven(this int number) => number % 2 == 0;
        public static bool IsOdd(this int number) => number % 2 != 0;

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
    }
}
