using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class TextureProcessor : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        string pathWithoutExtension = Path.GetFileNameWithoutExtension(assetPath);
        string suffix = pathWithoutExtension.Substring(pathWithoutExtension.Length - 2);

        TextureImporter importer = (TextureImporter)assetImporter;

        // Repeatable
        if(suffix == "_R")
        {
            importer.wrapMode = TextureWrapMode.Repeat;
        }

        // Background
        if(suffix == "_B")
        {
            importer.spritePixelsPerUnit = 100;
            importer.filterMode = FilterMode.Bilinear;
        }
        else
        {
            importer.spritePixelsPerUnit = 16;
            importer.filterMode = FilterMode.Point;
        }
        importer.textureCompression = TextureImporterCompression.Uncompressed;
    }
}
