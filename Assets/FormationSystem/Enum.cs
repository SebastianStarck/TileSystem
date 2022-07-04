using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Generic;

namespace FormationSystem
{
    public static class FormationPositionHelper
    {
        #region Non extension methods

        /// <summary>
        /// Get a FormationPosition out of a Vector3
        /// </summary>
        /// <param name="position">Target vector</param>
        /// <exception cref="NotImplementedException"></exception>
        public static FormationPosition FromVector3(Vector3 position)
        {
            return (FormationPosition) (position.z * Enum<FormationColumn>.Length + position.x);
        }

        /// <summary>
        /// Get a FormationPosition out of a Vector2
        /// </summary>
        /// <param name="position">Target vector</param>
        /// <exception cref="NotImplementedException"></exception>
        public static FormationPosition FromVector2(Vector2Int position)
        {
            return (FormationPosition) (position.y * Enum<FormationColumn>.Length + position.x);
        }

        /// <summary> Get a FormationPosition out of a row and a column </summary>
        /// <param name="row">Target row</param>
        /// <param name="column">Target column</param>
        public static FormationPosition FromRowAndColumn(FormationRow row, FormationColumn column)
        {
            return (FormationPosition) (row.ToInt() * Enum<FormationColumn>.Length + column.ToInt());
        }

        #endregion

        #region Itineration

        /// <summary> Gets the next FormationPosition </summary>
        public static FormationPosition GetNext(this FormationPosition position)
        {
            var val = (position + 1) <= Enum<FormationPosition>.End
                ? position + 1
                : Enum<FormationPosition>.Start;

            return val;
        }

        /// <summary> Gets the next FormationPosition </summary>
        public static FormationPosition GetPrevious(this FormationPosition position)
        {
            return (int) (position - 1) >= 0 ? position - 1 : Enum<FormationPosition>.End;
        }

        #endregion

        /// <summary>
        /// Calculate positions in range of a given position
        /// </summary>
        /// <param name="position">The point of origin</param>
        /// <param name="maxDistance">Max range between positions</param>
        /// <param name="startingRow">Starting row</param>
        /// <param name="rowModifier">Direct modifier to range</param>
        /// <returns></returns>
        /// TODO: This should stop if the row falls out of range
        public static IEnumerable<FormationPosition> GetPositionsInRange(
            this FormationPosition position,
            int maxDistance = 1,
            int? startingRow = default,
            int rowModifier = 0)
        {
            var (column, row) = position.ToVector2();

            var positions = new List<FormationPosition>();

            for (var i = 0; i < maxDistance - rowModifier; i++)
            {
                var currentRow = Mathf.Clamp(
                    (startingRow ?? Enum<FormationRow>.End.ToInt()) - i,
                    Enum<FormationRow>.Start.ToInt(),
                    Enum<FormationRow>.End.ToInt()
                );

                var start = Math.Max(column - (maxDistance - i - 1), 0);
                var end = Math.Min(column + (maxDistance - i - 1), Enum<FormationColumn>.End.ToInt());

                var positionsInRow = Util
                    .GetRange(start, end)
                    .Select(otherColumn => FromVector2(new Vector2Int(otherColumn, currentRow)));

                positions = positions.Concat(positionsInRow).ToList();
            }

            return positions;
        }

        // public static IEnumerable<FormationPosition> GetPositionsInRange(this FormationPosition position, int range)
        // {
        //     return new List<FormationPosition>();
        // }

        /// <summary> Gets the FormationPosition in the next row </summary>
        /// 
        public static FormationPosition GetPositionInNextRow(this FormationPosition position)
        {
            var (column, row) = position.ToVector2();

            return (FormationPosition) (Enum<FormationColumn>.Length * (row + 1) + column);
        }

        /// <summary> Get the FormationPosition in the previous row </summary>
        public static FormationPosition GetPositionInPreviousRow(this FormationPosition position)
        {
            var (column, row) = position.ToVector2();

            return (FormationPosition) (Enum<FormationColumn>.Length * (row - 1) + column);
        }

        /// <summary> Gets the opposite FormationPosition</summary>
        public static FormationPosition GetOpposite(this FormationPosition position)
        {
            var (column, row) = position.ToVector2();
            var mirroredColumn = Math.Abs(Enum<FormationColumn>.Length - column - 1);

            return FromRowAndColumn((FormationRow) row, (FormationColumn) mirroredColumn);
        }

        #region Casting

        /// <summary>
        /// Convert enum to its int equivalent.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int ToInt(this Enum val) => Convert.ToInt32(val);


        /// <summary> Convert a FormationPosition to Vector2 </summary>
        public static Vector2Int ToVector2(this FormationPosition val)
        {
            var row = val.ToInt() / Enum<FormationColumn>.Length;
            var column = val.ToInt() % Enum<FormationColumn>.Length;

            return new Vector2Int(column, row);
        }

        /// <summary> Convert a FormationPosition to Vector3 </summary>
        public static Vector3 ToVector3(this FormationPosition position, float verticalOffset = 0)
        {
            var row = position.ToInt() / Enum<FormationColumn>.Length;
            var column = position.ToInt() % Enum<FormationColumn>.Length;

            return new Vector3(column, verticalOffset, row);
        }

        #endregion
    }
}