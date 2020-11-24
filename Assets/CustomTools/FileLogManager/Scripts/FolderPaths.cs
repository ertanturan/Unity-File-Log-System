using System.IO;
using UnityEngine;

public static class FolderPaths
{
    private static string _rootPath = Path.Combine(Application.persistentDataPath, "Logs");

    public static string RootPath
    {
        get
        {
            if (!Directory.Exists(_rootPath))
            {
                Directory.CreateDirectory(_rootPath);
            }

            return _rootPath;
        }
    }
    //public static string TempPath = Path.Combine(_rootPath, "TEMP");
    private static string _saveDir = Application.persistentDataPath + "/saves/";

    public static string SaveDir
    {
        get
        {
            if (!Directory.Exists(_saveDir))
            {
                Directory.CreateDirectory(_saveDir);
            }

            return _saveDir;
        }
    }
}
