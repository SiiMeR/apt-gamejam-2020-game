using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

// when an UI asset is imported to Unity (default type) into the menu folder then it sets the type to 2D(UI and Sprite automatically)
public class UiSpritePreProcessor : AssetPostprocessor
{
    public static readonly List<string> TEXTURE_PATHS = new List<string>
    {
        "Assets/Sprites"
    };

    private void OnPreprocessTexture()
    {
        var textureImporter = (TextureImporter) assetImporter;

        var asset = AssetDatabase.LoadAssetAtPath(textureImporter.assetPath, typeof(Texture2D));

        if (!asset && textureImporter.textureType == TextureImporterType.Default && IsInAssetPath(textureImporter.assetPath))
        {
            Debug.Log($"Imported new texture at path {textureImporter.assetPath} and set it to type Sprite.");
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.filterMode = FilterMode.Point;
            textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
        }
    }

    private static bool IsInAssetPath(string path) => TEXTURE_PATHS.Any(path.Contains);
}

#endif