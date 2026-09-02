using UnityEngine;
using UnityEditor;

public class SpriteColorChanger : MonoBehaviour
{
    [MenuItem("Tools/Change Selected Sprites (White to Transparent)")]
    private static void ChangeSpriteWhiteToTransparent()
    {
        foreach (Object obj in Selection.objects)
        {
            if (obj is Texture2D texture)
            {
                string path = AssetDatabase.GetAssetPath(texture);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

                if (importer != null && importer.textureType == TextureImporterType.Sprite)
                {
                    importer.isReadable = true; // Ensure texture is readable
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

                    Texture2D newTexture = new Texture2D(texture.width, texture.height);
                    Color[] pixels = texture.GetPixels();

                    for (int i = 0; i < pixels.Length; i++)
                    {
                        if (pixels[i] == Color.white) // Keep alpha the same, modify RGB to white
                        {
                            pixels[i] = new Color(1, 1, 1, 0);
                        }
                    }

                    newTexture.SetPixels(pixels);
                    newTexture.Apply();

                    // Save the modified texture
                    byte[] bytes = newTexture.EncodeToPNG();
                    System.IO.File.WriteAllBytes(path, bytes);
                    AssetDatabase.ImportAsset(path);

                    Debug.Log("White converted to transparent: " + path);
                }
                else
                {
                    Debug.LogWarning("Selected texture is not a sprite or not readable: " + obj.name);
                }
            }
        }
    }

    [MenuItem("Tools/Change Selected Sprites (Black to Transparent)")]
    private static void ChangeSpriteBlackToTransparent()
    {
        foreach (Object obj in Selection.objects)
        {
            if (obj is Texture2D texture)
            {
                string path = AssetDatabase.GetAssetPath(texture);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

                if (importer != null && importer.textureType == TextureImporterType.Sprite)
                {
                    importer.isReadable = true; // Ensure texture is readable
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

                    Texture2D newTexture = new Texture2D(texture.width, texture.height);
                    Color[] pixels = texture.GetPixels();

                    for (int i = 0; i < pixels.Length; i++)
                    {
                        if (pixels[i] == Color.black) // Keep alpha the same, modify RGB to white
                        {
                            pixels[i] = new Color(0, 0, 0, 0);
                        }
                    }

                    newTexture.SetPixels(pixels);
                    newTexture.Apply();

                    // Save the modified texture
                    byte[] bytes = newTexture.EncodeToPNG();
                    System.IO.File.WriteAllBytes(path, bytes);
                    AssetDatabase.ImportAsset(path);

                    Debug.Log("Black converted to transparent: " + path);
                }
                else
                {
                    Debug.LogWarning("Selected texture is not a sprite or not readable: " + obj.name);
                }
            }
        }
    }

    [MenuItem("Tools/Change Selected Sprites to White")]
    private static void ChangeSpriteColorToWhite()
    {
        foreach (Object obj in Selection.objects)
        {
            if (obj is Texture2D texture)
            {
                string path = AssetDatabase.GetAssetPath(texture);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

                if (importer != null && importer.textureType == TextureImporterType.Sprite)
                {
                    importer.isReadable = true; // Ensure texture is readable
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

                    Texture2D newTexture = new Texture2D(texture.width, texture.height);
                    Color[] pixels = texture.GetPixels();

                    for (int i = 0; i < pixels.Length; i++)
                    {
                        if (pixels[i].a > 0) // Keep alpha the same, modify RGB to white
                        {
                            pixels[i] = new Color(1, 1, 1, pixels[i].a);
                        }
                    }

                    newTexture.SetPixels(pixels);
                    newTexture.Apply();

                    // Save the modified texture
                    byte[] bytes = newTexture.EncodeToPNG();
                    System.IO.File.WriteAllBytes(path, bytes);
                    AssetDatabase.ImportAsset(path);

                    Debug.Log("Sprite color changed to white: " + path);
                }
                else
                {
                    Debug.LogWarning("Selected texture is not a sprite or not readable: " + obj.name);
                }
            }
        }
    }

    [MenuItem("Tools/Invert RGB Colors")]
    private static void InvertColors()
    {
        foreach (Object obj in Selection.objects)
        {
            if (obj is Texture2D texture)
            {
                string path = AssetDatabase.GetAssetPath(texture);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

                if (importer != null && importer.textureType == TextureImporterType.Sprite)
                {
                    importer.isReadable = true; // Ensure texture is readable
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

                    Texture2D newTexture = new Texture2D(texture.width, texture.height);
                    Color[] pixels = texture.GetPixels();

                    for (int i = 0; i < pixels.Length; i++)
                    {
                        if (pixels[i].a > 0) // Keep alpha the same, modify RGB to white
                        {
                            pixels[i] = new Color(1 - pixels[i].r, 1 - pixels[i].g, 1 - pixels[i].b, pixels[i].a);
                        }
                    }

                    newTexture.SetPixels(pixels);
                    newTexture.Apply();

                    // Save the modified texture
                    byte[] bytes = newTexture.EncodeToPNG();
                    System.IO.File.WriteAllBytes(path, bytes);
                    AssetDatabase.ImportAsset(path);

                    Debug.Log("Sprite color changed to white: " + path);
                }
                else
                {
                    Debug.LogWarning("Selected texture is not a sprite or not readable: " + obj.name);
                }
            }
        }
    }
}