using System;
using System.Linq;
using UnityEngine;

namespace Generic
{
    /// <summary>
    /// GameObject's Extension methods Wrapper class
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Fetch a gameObject child by name
        /// </summary>
        /// <param name="childName">Name of the child</param>
        /// <param name="includeInactive">Include inactive children</param>
        /// <returns></returns>
        public static GameObject GetChildByName(this Transform gameObject, string childName,
            bool includeInactive = true)
        {
            if (gameObject.childCount == 0) return null;

            return gameObject.transform
                .GetComponentsInChildren<Transform>(includeInactive)
                .First(child => child.gameObject.name.ToLower() == childName.ToLower())
                .gameObject;
        }

        public static T GetComponentInChild<T>(this Transform gameObject, int childIndex)
        {
            if (gameObject.childCount == 0) return default;
            var child = gameObject.GetChild(childIndex);

            return child == null ? default : child.GetComponent<T>();
        }
    }
}