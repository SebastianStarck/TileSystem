using UnityEditor;
using UnityEngine;

public class AssetLoader<T> where T : class
{
    public static T LoadAsset(string name, string path)
    {
        return AssetDatabase.LoadAssetAtPath($"Assets/{path}/{name}", typeof(T)) as T;
    }
}