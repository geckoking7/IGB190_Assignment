using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class EditorUtilityExtensions
{
    /// <summary>
    /// Finds all ScriptableObjects of type T in the project.
    /// </summary>
    public static List<T> FindAllAssetsOfType<T>() where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
        List<T> assets = new List<T>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null)
                assets.Add(asset);
        }

        return assets;
    }

    public static bool IsActuallyModified(Object obj)
    {
        string assetPath = AssetDatabase.GetAssetPath(obj);
        if (string.IsNullOrEmpty(assetPath))
            return false; // not an asset

        // Read saved version from disk
        Object saved = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
        string savedJson = EditorJsonUtility.ToJson(saved);
        string currentJson = EditorJsonUtility.ToJson(obj);

        return savedJson != currentJson;
    }

    public static void RevertToSaved(Object asset)
    {
        string path = AssetDatabase.GetAssetPath(asset);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning("Object is not an asset on disk.");
            return;
        }

        // Force Unity to forget the current in-memory copy.
        AssetDatabase.ReleaseCachedFileHandles();
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        // Refresh ensures any inspectors and references update.
        AssetDatabase.Refresh();

        Debug.Log($"Reverted '{path}' to its last saved version on disk.");
    }

    public static void RevertSerializedData(Object asset)
    {
        string path = AssetDatabase.GetAssetPath(asset);
        if (string.IsNullOrEmpty(path))
            return;

        string fileText = File.ReadAllText(path);
        EditorJsonUtility.FromJsonOverwrite(fileText, asset);

        // Mark it clean again so Unity won’t resave on quit
        EditorUtility.ClearDirty(asset);

        Debug.Log($"Reverted serialized fields of {asset.name} from disk.");
    }

    public static string ComputeHash(Object obj)
    {
        if (obj == null) return string.Empty;

        // Convert object to JSON string (serialized form)
        string json = EditorJsonUtility.ToJson(obj);

        // Compute SHA256 hash
        using (SHA256 sha = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            byte[] hash = sha.ComputeHash(bytes);
            return System.Convert.ToBase64String(hash);
        }
    }

    public static T LoadFromDisk<T>(string assetPath) where T : ScriptableObject
    {
        // Uses Unity's internal deserializer to load a fresh copy
        Object[] objs = UnityEditorInternal.InternalEditorUtility.LoadSerializedFileAndForget(assetPath);
        if (objs == null || objs.Length == 0)
            return null;

        T result = objs[0] as T;
        return result;
    }
}