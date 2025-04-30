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
        ollamaExecutable = Path.Combine(backendPath, "Ollama.app");
        modelPath = Path.Combine(Application.dataPath, "..", "PythonBackend", "common", "GGUF_Models");

#elif UNITY_STANDALONE_OSX
        isWindows = false;
        string appRoot = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", "..")); // MyGame.app/
        backendPath = Path.Combine(appRoot, "PythonBackend", "macos");
        serverExecutable = Path.Combine(backendPath, "dist", "chromadb_fastapi", "chromadb_fastapi");
        ollamaExecutable = Path.Combine(backendPath, "Ollama.app");
        modelPath = Path.Combine(appRoot, "PythonBackend", "common", "GGUF_Models");
#endif
    }


    private void StartProcesses()
    {
        try
        {
            ProcessStartInfo ollamaStartInfo;

            if (isWindows)
            {
                ollamaStartInfo = new ProcessStartInfo
                {
                    FileName = ollamaExecutable,
                    WorkingDirectory = backendPath,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
            }
            else
            {
                // Launch the macOS app bundle using `open -a Ollama.app --args serve`
                ollamaStartInfo = new ProcessStartInfo
                {
                    FileName = "open",
                    Arguments = $"-a \"{ollamaExecutable}\" --args serve",
                    WorkingDirectory = backendPath,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
            }

            if (!IsProcessRunning("Ollama"))
            {
                ollamaProcess = Process.Start(ollamaStartInfo);
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
                CreateNoWindow = true,
            };

            serverStartInfo.EnvironmentVariables["OLLAMA_MODELS"] = modelPath;

            serverProcess = Process.Start(serverStartInfo);
            Debug.Log("FastAPI server started.");
        }
        catch (Exception ex)
        {
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
