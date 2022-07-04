using System.Collections.Generic;
using UnityEngine;

namespace Generic
{
    public static class Util
    {
        /// <summary>
        /// Get an array of ints between a given array
        /// </summary>
        /// <param name="from">Sequence start, inclusive</param>
        /// <param name="to">Sequence end, inclusive</param>
        /// <returns></returns>
        public static int[] GetRange(int from, int to)
        {
            var list = new List<int>();

            for (var i = from; i <= to; i++) list.Add(i);

            return list.ToArray();
        }

    }
}