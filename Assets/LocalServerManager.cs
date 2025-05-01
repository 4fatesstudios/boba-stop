using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class LocalServerManager : MonoBehaviour
{
    public static LocalServerManager Instance { get; private set; }
    
    private Process ollamaProcess;
    private Process serverProcess;

    private string backendPath;
    private string modelPath;
    private string serverExecutable;
    private string ollamaExecutable;
    private bool isWindows;
    
    private string logBuffer = "";
    private Vector2 scrollPos;
    private string logFilePath = "local_server_log.txt";  // Path to the log file

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // make persistent across scenes
        }
        else {
            Destroy(gameObject); // delete duplicates
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        SetupPaths();
        StartProcesses();
    }
    
    private void SetupPaths()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        isWindows = true;
        backendPath = Path.Combine(Application.dataPath, "..", "PythonBackend", "windows");
        serverExecutable = Path.Combine(backendPath, "dist", "chromadb_fastapi", "chromadb_fastapi.exe");
        ollamaExecutable = Path.Combine(backendPath, "ollama app.exe");
        modelPath = Path.Combine(Application.dataPath, "..", "PythonBackend", "common", "GGUF_Models");

#elif UNITY_EDITOR_OSX
        isWindows = false;
        backendPath = Path.Combine(Application.dataPath, "..", "PythonBackend", "macos");
        serverExecutable = Path.Combine(backendPath, "dist", "chromadb_fastapi", "chromadb_fastapi");
        ollamaExecutable = Path.Combine(backendPath, "ollama");
        modelPath = Path.Combine(Application.dataPath, "..", "PythonBackend", "common", "GGUF_Models");

#elif UNITY_STANDALONE_OSX
        isWindows = false;
        string appRoot = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", "..")); // MyGame.app/
        backendPath = Path.Combine(appRoot, "PythonBackend", "macos");
        serverExecutable = Path.Combine(backendPath, "dist", "chromadb_fastapi", "chromadb_fastapi");
        ollamaExecutable = Path.Combine(backendPath, "ollama");
        modelPath = Path.Combine(appRoot, "PythonBackend", "common", "GGUF_Models");
#endif
    }

    private void StartProcesses()
    {
        try
        {
            var ollamaStartInfo = new ProcessStartInfo
            {
                FileName = ollamaExecutable,
                WorkingDirectory = backendPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            if (!IsProcessRunning(Path.GetFileNameWithoutExtension(ollamaExecutable)))
            {
                ollamaProcess = Process.Start(ollamaStartInfo);
                ollamaProcess.OutputDataReceived += (s, e) => { if (e.Data != null) LogMessage($"[Ollama] {e.Data}"); };
                ollamaProcess.ErrorDataReceived += (s, e) => { if (e.Data != null) LogMessage($"[Ollama-ERR] {e.Data}"); };
                ollamaProcess.BeginOutputReadLine();
                ollamaProcess.BeginErrorReadLine();

                Debug.Log("Ollama started.");
            }
            else
            {
                Debug.LogWarning("Ollama already running.");
            }

            var serverStartInfo = new ProcessStartInfo
            {
                FileName = serverExecutable,
                WorkingDirectory = backendPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            serverStartInfo.EnvironmentVariables["OLLAMA_MODELS"] = modelPath;

            serverProcess = Process.Start(serverStartInfo);
            serverProcess.OutputDataReceived += (s, e) => { if (e.Data != null) LogMessage($"[Server] {e.Data}"); };
            serverProcess.ErrorDataReceived += (s, e) => { if (e.Data != null) LogMessage($"[Server-ERR] {e.Data}"); };
            serverProcess.BeginOutputReadLine();
            serverProcess.BeginErrorReadLine();

            Debug.Log("FastAPI server started.");
        }
        catch (Exception ex)
        {
            LogMessage($"[ERROR] Failed to start processes: {ex.Message}");
            Debug.LogError($"Failed to start processes: {ex.Message}");
        }
    }

    private bool IsProcessRunning(string processName)
    {
        foreach (var p in Process.GetProcessesByName(processName))
        {
            if (!p.HasExited) return true;
        }
        return false;
    }

    private void LogMessage(string message)
    {
        logBuffer += message + "\n";

        // Output to the log file
        try
        {
            File.AppendAllText(logFilePath, message + "\n");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error writing to log file: {ex.Message}");
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("API & Ollama Logs", GUILayout.Height(20));
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUILayout.Label(logBuffer, GUI.skin.label);
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    private void OnApplicationQuit()
    {
        try
        {
            if (serverProcess != null && !serverProcess.HasExited)
            {
                serverProcess.Kill();
                serverProcess.Dispose();
                Debug.Log("Server process terminated.");
            }

            if (ollamaProcess != null && !ollamaProcess.HasExited)
            {
                ollamaProcess.Kill();
                ollamaProcess.Dispose();
                Debug.Log("Ollama process terminated.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error during shutdown: {ex.Message}");
        }
    }
}
