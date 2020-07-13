using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public static class FileLogger
{

    private static string _staticPath = Path.Combine(Application.persistentDataPath, "Logs");

    private static string _fileName;

    [RuntimeInitializeOnLoadMethod]
    private static void OnLoad()
    {
        CreateDirectory();
        CreateLogFile();
    }

    private static void CreateDirectory()
    {
        if (!Directory.Exists(_staticPath))
        {
            Directory.CreateDirectory(_staticPath);
        }
    }

    private static void CreateLogFile()
    {

        FileInfo temp = new FileInfo(Path.Combine(_staticPath, FileName()));

        temp.Create();

        Debug.Log(temp.Directory);
        Process.Start(temp.DirectoryName);

    }

    private static void CheckLogFile()
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            _fileName = FileName();
        }

        if (!File.Exists(Path.Combine(_staticPath, _fileName)))
        {
            CreateLogFile();
        }

    }

    private static string FileName()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(SceneManager.GetActiveScene().name);
        sb.Append("_");
        sb.Append(DateTime.Now.Hour);
        sb.Append("_");
        sb.Append(DateTime.Now.Day);
        sb.Append("_");
        sb.Append(DateTime.Now.Month);
        sb.Append("_");
        sb.Append(DateTime.Now.Year);
        sb.Append("_");
        sb.Append(Guid.NewGuid().ToString());

        _fileName = sb + ".txt";

        return _fileName;
    }

    public static void LogEntry(string entry)
    {

        CheckLogFile();

        using (StreamWriter stream = new StreamWriter(Path.Combine(_staticPath, _fileName), true))
        {
            stream.WriteLine(DateTime.Now + "------" + entry);

        }

    }
}
