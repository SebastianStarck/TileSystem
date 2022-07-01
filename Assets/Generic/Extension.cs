using UnityEngine;

namespace Generic
{
    public static class MathExtensions
    {
        public static bool IsEven(this int number) => number % 2 == 0;
        public static bool IsOdd(this int number) => number % 2 != 0;
    }
}