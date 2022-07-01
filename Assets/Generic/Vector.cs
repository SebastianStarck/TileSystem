using UnityEngine;

namespace Generic
{
    public static class VectorExtension
    {
        public static bool IsEven(this Vector2Int point) => point.x.IsEven() && point.y.IsEven();
        public static bool IsOdd(this Vector2Int point) => point.x.IsOdd() && point.y.IsOdd();

        public static Vector2Int ToVector2(this Vector3 point) => new Vector2Int((int) point.x, (int) point.y);
        public static Vector3 ToVector3(this Vector2Int point) => new Vector3(point.x, 0f, point.y);

        public static Vector3 AddY(this Vector3 point, float amount) => point + new Vector3(0, amount);

        public static void Deconstruct(this Vector2Int vector, out int x, out int y)
        {
            x = vector.x;
            y = vector.y;
        }
    }
}