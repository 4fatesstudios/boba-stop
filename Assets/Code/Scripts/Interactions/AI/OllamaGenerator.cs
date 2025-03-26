using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChromaDB.Client;
using Microsoft.Extensions.AI;
using System.Net.Http;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using BobaStop.NPCs;

public class OllamaGenerator
{
    private ChromaClient client;
    private HttpClient httpClient;
    private ChromaCollectionClient chromaDBClient;
    private ChromaDBRetriever retriever;
    private IChatClient chatClient;
    private List<ChatMessage> chatHistory = new();

    private CompanionData companionData;

    public OllamaGenerator(CompanionData companionData) {
        this.companionData = companionData;
    }

    public async void Start()
    {
        // StartChromaDBServer();
        // await InitializeChromaClient();
        // string first = await StartChat();
        await StartChat();
        // UnityEngine.Debug.Log(first);
    }

    async Task InitializeChromaClient()
    {
        UnityEngine.Debug.Log("Initializing ChromaDB Client...");
        
        var configOptions = new ChromaConfigurationOptions(uri: "http://localhost:8000/api/v1/");
        httpClient = new HttpClient();
        client = new ChromaClient(configOptions, httpClient);

        var collectionName = companionData.companionName.ToLower() + "_personality";

        var collection = await client.GetOrCreateCollection(collectionName);
        chromaDBClient = new ChromaCollectionClient(collection, configOptions, httpClient);

        UnityEngine.Debug.Log("ChromaDB Client Initialized for " + companionData.companionName);
    }

    async Task<string> StartChat()
    {
        chatClient = new OllamaChatClient(new Uri("http://localhost:11434/"), "llama3.2");
        UnityEngine.Debug.Log("Chatbot Initialized. Ready to chat!");

        string current_conext = "First interaction with " + companionData.companionName;

        // var retrieved_context = await retriever.RetrieveData(chromaDBClient, current_conext);
        var retrieved_context = "No previous interactions";

        var prompt = $"You are {companionData.companionName}, an NPC in a game with relationships from Rival (-3) to lover (3). Your rapport level with the player is {companionData.rapportLevel} out of 5. Your current situation is {current_conext}. Your previous interactions: {retrieved_context}. I want you to generate one line of {companionData.companionName}’s response to this situation in a natural, engaging way.";

        chatHistory.Add(new ChatMessage(ChatRole.User, prompt));

        // Stream the AI response and add to chat history
        UnityEngine.Debug.Log("AI Response: ");
        var response = "";
        await foreach (var item in
            chatClient.GetStreamingResponseAsync(chatHistory))
        {
            // UnityEngine.Debug.Log(item.Text);
            response += item.Text;
        }
        chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));
        UnityEngine.Debug.Log(response);

        return response;
    }

    public async Task<string> Chat(string userPrompt) {
        // Get user prompt and add to chat history
        chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

        // Stream the AI response and add to chat history
        UnityEngine.Debug.Log("AI Response: ");
        var response = "";
        await foreach (var item in
            chatClient.GetStreamingResponseAsync(chatHistory))
        {
            // UnityEngine.Debug.Log(item.Text);
            response += item.Text;
        }
        chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));
        UnityEngine.Debug.Log(response);

        return response;
    }

    static void StartChromaDBServer()
    {
        try
        {
            // 1. Use the correct executable name and path
            string chromaPath = "/Library/Frameworks/Python.framework/Versions/3.12/bin/chroma"; // From 'which chroma' result
            
            // 2. Set up paths
            string projectRoot = Path.GetDirectoryName(Application.dataPath);
            string chromaStoragePath = Path.Combine(projectRoot, "chroma_db");
            
            // 3. Create directory if needed
            if (!Directory.Exists(chromaStoragePath))
                Directory.CreateDirectory(chromaStoragePath);

            // 4. Configure process
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = chromaPath,
                Arguments = $"run --path \"{chromaStoragePath}\"",
                WorkingDirectory = projectRoot,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // 5. Debug output
            UnityEngine.Debug.Log($"Starting Chroma: {chromaPath} {psi.Arguments}");

            // 6. Start process
            using (Process process = new Process { StartInfo = psi })
            {
                process.OutputDataReceived += (sender, e) => UnityEngine.Debug.Log(e.Data);
                process.ErrorDataReceived += (sender, e) => UnityEngine.Debug.LogError(e.Data);
                
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Chroma start failed: {ex.Message}");
        }
    }
}
