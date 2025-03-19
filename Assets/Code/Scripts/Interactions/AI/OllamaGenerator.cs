using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChromaDB.Client;
using Microsoft.Extensions.AI;
using System.Net.Http;
using System.Diagnostics;
using BobaStop.NPCs;

public class OllamaGenerator : MonoBehaviour
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
        StartChromaDBServer();
        await InitializeChromaClient();
        await StartChat();
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

        var retrieved_context = await retriever.RetrieveData(chromaDBClient, current_conext);

        var prompt = $"You are {companionData.companionName}, an NPC in a game with relationships from Rival (-3) to lover (3). Your rapport level with the player is {companionData.rapportLevel} out of 5. Your current situation is {current_conext}. Your previous interactions: {retrieved_context}. I want you to generate one line of {companionData.companionName}’s response to this situation in a natural, engaging way.";

        chatHistory.Add(new ChatMessage(ChatRole.User, prompt));

        // Stream the AI response and add to chat history
        UnityEngine.Debug.Log("AI Response: ");
        var response = "";
        await foreach (var item in
            chatClient.GetStreamingResponseAsync(chatHistory))
        {
            UnityEngine.Debug.Log(item.Text);
            response += item.Text;
        }
        chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));

        return response;
    }

    public async Task<string> Chat(string userPrompt) {
        // Get user prompt and add to chat history
        chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

        // Stream the AI response and add to chat history
        UnityEngine.Debug.Log("AI Response:");
        string response = "";
        await foreach (var item in
            chatClient.GetStreamingResponseAsync(chatHistory))
        {
            UnityEngine.Debug.Log(item.Text);
            response += item.Text;
        }
        chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));
        return response;
    }

        static void StartChromaDBServer()
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "chromadb", // If using Windows, replace with "python"
                Arguments = "run --path ../chroma_db",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process process = new Process { StartInfo = psi };
            process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
            process.ErrorDataReceived += (sender, e) => Console.WriteLine("ERROR: " + e.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            Console.WriteLine("ChromaDB server started...");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error starting ChromaDB: " + ex.Message);
        }
    }
}
