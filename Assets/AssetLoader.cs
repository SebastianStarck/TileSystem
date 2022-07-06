using System.Linq;
using UnityEditor;
using UnityEngine;

public static class AssetLoader
{
    public static T LoadAsset<T>(string name, string path) where T : class => AssetDatabase.LoadAssetAtPath($"Assets/{path}/{name}", typeof(T)) as T;
    public static T[] LoadAssets<T>(string path) => AssetDatabase.LoadAllAssetsAtPath($"Assets/{path}").Cast<T>().ToArray();
}
