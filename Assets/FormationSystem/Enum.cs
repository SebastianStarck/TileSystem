using System;
using UnityEngine;
using Generic;

namespace FormationSystem
{
    public static class FormationPositionHelper
    {
        /// <summary>
        /// Get a FormationPosition out of a Vector3
        /// </summary>
        /// <param name="position">Target vector</param>
        /// <exception cref="NotImplementedException"></exception>
        public static FormationPosition FromVector3(Vector3 position)
        {
            return (FormationPosition) (position.z * Enum<FormationColumn>.Length + position.x);
        }

        /// <summary> Get a FormationPosition out of a row and a column </summary>
        /// <param name="row">Target row</param>
        /// <param name="column">Target column</param>
        public static FormationPosition FromRowAndColumn(FormationRow row, FormationColumn column)
        {
            return (FormationPosition) (row.ToInt() * Enum<FormationColumn>.Length + column.ToInt());
        }

        #region Extension methods

        /// <summary>
        /// Convert enum to its int equivalent.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int ToInt(this Enum val) => Convert.ToInt32(val);

        /// <summary> Gets the next FormationPosition </summary>
        public static FormationPosition GetNext(this FormationPosition position)
        {
            var val = (int) (position + 1) <= Enum<FormationPosition>.Length - 1
                ? position + 1
                : Enum<FormationPosition>.Start;

            return val;
        }

        /// <summary> Gets the next FormationPosition </summary>
        public static FormationPosition GetPrevious(this FormationPosition position)
        {
            return (int) (position - 1) >= 0 ? position - 1 : Enum<FormationPosition>.End;
        }

        /// <summary> Gets the FormationPosition in the next row </summary>
        public static FormationPosition GetNextX(this FormationPosition position)
        {
            var (column, row) = position.ToVector2();

            return (FormationPosition) (Enum<FormationColumn>.Length * (row + 1) + column);
        }

        /// <summary> Gets the opposite FormationPosition</summary>
        public static FormationPosition GetOpposite(this FormationPosition position)
        {
            var (column, row) = position.ToVector2();
            var mirroredColumn = Math.Abs(Enum<FormationColumn>.Length - column - 1);

            return FromRowAndColumn((FormationRow) row, (FormationColumn) mirroredColumn);
        }

        /// <summary> Get the FormationPosition in the previous row </summary>
        public static FormationPosition GetPreviousX(this FormationPosition position)
        {
            var (column, row) = position.ToVector2();

            return (FormationPosition) (Enum<FormationColumn>.Length * (row - 1) + column);
        }

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