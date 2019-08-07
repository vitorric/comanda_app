using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager
{
    public enum Directories
    {
        produto,
        item_Loja,
        desafio
    }

    public static string FilePath(Directories directory, string fileName)
    {
        return Path.Combine(Application.persistentDataPath, "Files", directory.ToString(), fileName);
    }

    public static void CreateFileDirectory(Directories directory, bool deleteFiles = true)
    {
        string path = FilePath(directory, string.Empty);

        if (Directory.Exists(path) && deleteFiles)
        {
            RemoveAllFiles();
        }

        Directory.CreateDirectory(path);
    }

    public static bool Exists(Directories directory, string fileName)
    {
        string path = FilePath(directory, fileName);
        return File.Exists(path);
    }

    public static void SaveFile(Directories directory, string fileName, byte[] data)
    {
        string path = FilePath(directory, fileName);
        File.WriteAllBytes(path, data);
    }

    public static byte[] LoadFile(Directories directory, string fileName)
    {
        byte[] data = null;

        try
        {
            string path = FilePath(directory, fileName);
            data = File.ReadAllBytes(path);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"FileManager.LoadFile erro: {ex.Message}");
        }

        return data;
    }

    public static void RemoveAllFiles()
    {
        try
        {
            Directory.Delete(FilePath(Directories.desafio, string.Empty), true);
            Directory.Delete(FilePath(Directories.item_Loja, string.Empty), true);
            Directory.Delete(FilePath(Directories.produto, string.Empty), true);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"FileManager.RemoveAllFiles erro: {ex.Message}");
        }
    }    

    public static Texture2D ConvertToTexture2D(byte[] bytes, int width = 1, int height = 1)
    {
        Texture2D tex = new Texture2D(width, height);
        tex.LoadImage(bytes);
        tex.Apply();
        return tex;
    }
}
