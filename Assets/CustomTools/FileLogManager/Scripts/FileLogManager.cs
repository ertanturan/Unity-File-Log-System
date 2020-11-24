using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class FileLogManager : MonoBehaviour
{

    private const string CUSTOM_LOG_FILENAME = "CustomLog";
    private const string CONSOLE_LOG_FILENAME = "CONSOLE_LOG";
    private const string PARENT_DIRECTORY_NAME = "LOGS";

    private static string _customLogFileName;
    private static string _consoleLogFileName;

    private static string _parentPath;

    private static StreamWriter stream;

    private static List<string> _customLogs = new List<string>();
    private static List<string> _consoleLogs = new List<string>();
    public KeyCode ShowDirectoryKey = KeyCode.L;

    private void Awake()
    {
        CreateDirectory();
        CreateLogFile(CONSOLE_LOG_FILENAME);
        CreateLogFile(CUSTOM_LOG_FILENAME);
#if UNITY_EDITOR
        LogEntry("  SCENE LOADED IN __EDITOR__  ");

#elif UNITY_STANDALONE

        LogEntry("SCENE LOADED IN __BUILD__  ");
#endif
        LogEntry("Loaded..");


        //ShowDirectory();
        Application.logMessageReceived += OnLogMessageReceived;

        InvokeRepeating("RoutineCheck", 10, 10);


    }


    private void OnDestroy()
    {

        Application.logMessageReceived -= OnLogMessageReceived;


    }


    private static void CreateDirectory()
    {
        if (!Directory.Exists(FolderPaths.RootPath))
        {
            Directory.CreateDirectory(FolderPaths.RootPath);
        }

        string filePath = Path.Combine(FolderPaths.RootPath, PARENT_DIRECTORY_NAME, FolderName());
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        _parentPath = filePath;
    }

    private static void CreateLogFile(string logFileName)
    {
        string fileName = FileName(logFileName);
        FileInfo temp = new FileInfo(Path.Combine(FolderPaths.RootPath, _parentPath, fileName));

        temp.Create();

        if (logFileName == CONSOLE_LOG_FILENAME)
        {
            _consoleLogFileName = fileName;
        }
        else if (logFileName == CUSTOM_LOG_FILENAME)
        {
            _customLogFileName = fileName;
        }



    }

    private static void CheckLogFile()
    {
        if (string.IsNullOrEmpty(_customLogFileName))
        {
            _customLogFileName = FileName(CUSTOM_LOG_FILENAME);
        }
        if (!File.Exists(Path.Combine(_parentPath, _customLogFileName)))
        {
            CreateLogFile(CUSTOM_LOG_FILENAME);
        }



        if (string.IsNullOrEmpty(_consoleLogFileName))
        {
            _consoleLogFileName = FileName(CONSOLE_LOG_FILENAME);
        }
        if (!File.Exists(Path.Combine(_parentPath, _consoleLogFileName)))
        {
            CreateLogFile(CONSOLE_LOG_FILENAME);
        }
    }

    private static string FileName(string startsWith)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(startsWith);
        sb.Append("_");

        sb.Append(FolderName());

        sb.Append(".txt");

        return sb.ToString();
    }

    private static string FolderName()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(DateTime.Now.Hour);
        sb.Append("_");
        sb.Append(DateTime.Now.Minute);
        sb.Append("_");
        sb.Append(DateTime.Now.Day);
        sb.Append("_");
        sb.Append(DateTime.Now.Month);
        sb.Append("_");
        sb.Append(DateTime.Now.Year);
        sb.Append("_");
        sb.Append(Guid.NewGuid().ToString());

        return sb.ToString();

    }

    public static void LogEntry(string rawentry)
    {

        string entry = DateTime.Now + "------" + rawentry;


        _customLogs.Add(entry);



    }

    private static void LogConsoleEntry(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception /*|| type == LogType.Warning*/)
        {
            if (!string.IsNullOrEmpty(logString) && !string.IsNullOrEmpty(stackTrace))
            {
                _consoleLogs.Add("\n");
                _consoleLogs.Add("--------------");
                _consoleLogs.Add("LOG TYPE : " + type.ToString());
                _consoleLogs.Add("\n");
                _consoleLogs.Add(logString);
                _consoleLogs.Add("\n\n STACK:");
                _consoleLogs.Add(stackTrace);
                _consoleLogs.Add("--------------");
                _consoleLogs.Add("\n");
            }
        }

    }

    public static void ShowDirectory()
    {
        if (!string.IsNullOrEmpty(FolderPaths.RootPath))
        {
            Process.Start(FolderPaths.RootPath);
        }
    }

    private void OnApplicationQuitted(object sender, EventArgs args)
    {
        _customLogs.Add("QUIT.");

        LogAll();


    }


    private void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            _customLogs.Add("SOFTWARE PAUSED.");
        }
        else
        {
            _customLogs.Add("SOFTWARE UN-PAUSED.");
        }
        LogAll();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            _customLogs.Add("SOFTWARE FOCUSED");
        }
        else
        {
            _customLogs.Add("SOFTWARE UN-FOCUSED");
        }
        LogAll();
    }

    private void LogAll()
    {
        StringBuilder customLogs = new StringBuilder();

        for (int i = 0; i < _customLogs.Count; i++)
        {
            customLogs.Append(_customLogs[i]);
            customLogs.Append("\n");
        }


        StringBuilder consoleLogs = new StringBuilder();

        for (int i = 0; i < _consoleLogs.Count; i++)
        {
            consoleLogs.Append(_consoleLogs[i]);
        }

        CheckLogFile();

        try
        {
            using (stream = new StreamWriter(Path.Combine(_parentPath, _customLogFileName), true))
            {
                stream.WriteLine(customLogs.ToString());
                stream.Flush();
            }

            _customLogs.Clear();


            using (stream = new StreamWriter(Path.Combine(_parentPath, _consoleLogFileName), true))
            {
                stream.WriteLine(consoleLogs.ToString());
                stream.Flush();
            }

            _consoleLogs.Clear();

        }
        catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
        {
            //LogEntry(rawentry);
        }
    }


    private static void OnLPressed()
    {
        ShowDirectory();
    }

    private void RoutineCheck()
    {
        LogAll();
    }

    protected virtual void OnLogMessageReceived(string logString, string stackTrace, LogType type)
    {

        LogConsoleEntry(logString, stackTrace, type);
    }


    private void Update()
    {
        if (Input.GetKeyDown(ShowDirectoryKey))
        {
            ShowDirectory();
        }
    }



}
