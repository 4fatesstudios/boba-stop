using System;
using System.Collections;
using BobaStop.NPCs;
using BobaStop.Context;
using UnityEngine;
using UnityEngine.Networking;


namespace BobaStop.AI {
    [System.Serializable]
    public class ChatPrompt {
        public string text;
    }

    [System.Serializable]
    public class ConversationRequest {
        public int rapport_level;
        public int rapport_level_progress;
        public string current_story;
    }

    public class CombinedData {
        public string name;
        public string age;
        public string role;
        public string livingCondition;
        public string personality;
        public string beliefs;
        public string speakingStyle;
        public string knowledgeScope;
        public string backstory;
        public string memory;
        public int rapportLevel;
        public string locationKnowledge;
        public string worldLocation;
        public string worldTime;
        public string worldWeather;
        public string currentLevel;
    }

    [System.Serializable]
    public class ConversationResponse {
        public string response;
        public string goodbye = "error";
    }

    public class APIClient : MonoBehaviour {
        public static APIClient Instance { get; private set; }
        
        [Header("FastAPI Settings")]
        public string baseUrl = "http://127.0.0.1:8000";

        private string data;

        private string goodbye;

        public bool IsReady { get; private set; } = false;
        public float startupTimeout = 60f; // configurable in Inspector
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject); // make persistent across scenes
            }
            else {
                Destroy(gameObject); // delete duplicates
            }
        }

        private void Start() {
            StartCoroutine(CheckServerReady());
        }

        private IEnumerator CheckServerReady() {
            float startTime = Time.time;
            string url = $"{baseUrl}/";

            while (Time.time - startTime < startupTimeout) {
                using (UnityWebRequest request = UnityWebRequest.Get(url)) {
                    yield return request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200) {
                        // Optional: check actual JSON body for "message": "Hello, World!"
                        var json = request.downloadHandler.text;
                        if (json.Contains("Hello, World")) {
                            IsReady = true;
                            Debug.Log("APIClient is ready.");
                            yield break;
                        }
                    }
                }

                yield return new WaitForSeconds(1f); // retry after 1 second
            }

            Debug.LogError("APIClient server startup timed out.");
        }

        // GET: /first_meeting/character/{character_name}
        public void GetFirstMeeting(CompanionData characterData, NPCDialogueData npcData, Action<string, string> onComplete) {
            if (characterData == null)
                Debug.Log("character data is null.");
            else 
                Debug.Log("character data not null");
            if (npcData == null)
                Debug.Log("npcdata is null");
            else 
                Debug.Log("npcdata  not null");
            
            if (onComplete != null)
                Debug.LogWarning("onComplete callback is not null.");
            else
                Debug.LogWarning("onComplete callback is null.");

            
            StartCoroutine(GetFirstMeetingIEnumerator(characterData, npcData, onComplete));
        }

        private IEnumerator GetFirstMeetingIEnumerator(CompanionData characterData, NPCDialogueData npcData, Action<string, string> onComplete) {
            string url = $"{baseUrl}/first_meeting/character/{characterData.companionName}";

            CombinedData requestData = new CombinedData {
                name = characterData.companionName,
                age = npcData.age,
                role = npcData.role,
                livingCondition = npcData.livingCondition,
                personality = npcData.personality,
                beliefs = npcData.beliefs,
                speakingStyle = npcData.speakingStyle,
                knowledgeScope = npcData.knowledgeScope,
                backstory = npcData.backstory,
                memory = npcData.memory,
                rapportLevel = (int)characterData.rapportLevel,
                // locationKnowledge = npcData.locationKnowledge[WorldContextManager.GetCurrentLevelAreaName()],
                worldLocation = WorldContextManager.GetCurrentLevelAreaName(),
                worldTime = WorldContextManager.GetTime(),
                worldWeather = WorldContextManager.GetWeather(),
                currentLevel = WorldContextManager.GetCurrentLevelContext()[0]
            };

            string json = JsonUtility.ToJson(requestData);

            using (UnityWebRequest request = new UnityWebRequest(url, "POST")) {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success) {
                    string jsonResponse = request.downloadHandler.text;
                    ConversationResponse response = JsonUtility.FromJson<ConversationResponse>(jsonResponse);
                    Debug.Log("First meeting response: " + response.response);
                    data = response.response;
                    goodbye = response.goodbye;
                } else {
                    Debug.LogError("GET error: " + request.error);
                    data = $"{characterData.companionName} is asleep right now. Please come back later!";
                }
            }
            onComplete?.Invoke(data, goodbye);
        }

        // POST: /new_conversation/character/{character_name}

        public void SendNewConversation(CompanionData characterData, NPCDialogueData npcData, Action<string, string> onComplete) {
            StartCoroutine(SendNewConversationIEnumerator(characterData, npcData, onComplete));
        }
        private IEnumerator SendNewConversationIEnumerator(CompanionData characterData, NPCDialogueData npcData, Action<string, string> onComplete) {
            string url = $"{baseUrl}/new_conversation/character/{characterData.companionName}";

            CombinedData requestData = new CombinedData {
                name = characterData.companionName,
                age = npcData.age,
                role = npcData.role,
                livingCondition = npcData.livingCondition,
                personality = npcData.personality,
                beliefs = npcData.beliefs,
                speakingStyle = npcData.speakingStyle,
                knowledgeScope = npcData.knowledgeScope,
                backstory = npcData.backstory,
                memory = npcData.memory,
                rapportLevel = (int)characterData.rapportLevel,
                // locationKnowledge = npcData.locationKnowledge[WorldContextManager.GetCurrentLevelAreaName()],
                // locationKnowledge = "This is a new Boba Shop in town.",
                worldLocation = WorldContextManager.GetCurrentLevelAreaName(),
                worldTime = WorldContextManager.GetTime(),
                worldWeather = WorldContextManager.GetWeather(),
                currentLevel = WorldContextManager.GetCurrentLevelContext()[1]
            };

            string json = JsonUtility.ToJson(requestData);

            using (UnityWebRequest request = new UnityWebRequest(url, "POST")) {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success) {
                    string jsonResponse = request.downloadHandler.text;
                    ConversationResponse response = JsonUtility.FromJson<ConversationResponse>(jsonResponse);
                    Debug.Log("New conversation response: " + response.response);
                    data = response.response;
                    goodbye = response.goodbye;
                } else {
                    Debug.LogError("POST error: " + request.error);
                    data = $"{characterData.companionName} is asleep right now. Please come back later!";
                }
                // Handle the response as needed
                onComplete?.Invoke(data, goodbye);
            }
        }

        // POST: /chat/character/{character_name}
        public void SendChatMessage(string characterName, string prompt, Action<string, string> onComplete) {
            StartCoroutine(SendChatMessageIEnumerator(characterName, prompt, onComplete));
        }
        private IEnumerator SendChatMessageIEnumerator(string characterName, string prompt, Action<string, string> onComplete) {
            string url = $"{baseUrl}/chat/character/{characterName}";
            Debug.Log("Sending chat message to: " + url);
            Debug.Log("Chat message: " + prompt);

            ChatPrompt chatPrompt = new ChatPrompt {
                text = prompt
            };

            string json = JsonUtility.ToJson(chatPrompt);

            using (UnityWebRequest request = new UnityWebRequest(url, "POST")) {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success) {
                    string jsonResponse = request.downloadHandler.text;
                    ConversationResponse response = JsonUtility.FromJson<ConversationResponse>(jsonResponse);
                    Debug.Log("Chat response: " + response.response);
                    data = response.response;
                    goodbye = response.goodbye;
                } else {
                    Debug.LogError("Chat POST error: " + request.error);
                    data = $"{characterName} is asleep right now. Please come back later!";
                }
            }

            onComplete?.Invoke(data, goodbye);
        }

        // POST: /summarize_chat/character/{character}
        // Not tested yet
        public void SendSummarizeChat(string characterName, Action<string> onComplete) {
            StartCoroutine(SendSummarizeChatIEnumerator(characterName, onComplete));
        }

        private IEnumerator SendSummarizeChatIEnumerator(string characterName, Action<string> onComplete) {
            string url = $"{baseUrl}/summarize_chat/character/{characterName}";

            Debug.Log("Sending chat message to: " + url);

            using (UnityWebRequest request = new UnityWebRequest(url, "GET")) {
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success) {
                    string jsonResponse = request.downloadHandler.text;
                    ConversationResponse response = JsonUtility.FromJson<ConversationResponse>(jsonResponse);
                    Debug.Log("Chat response: " + response.response);
                    data = response.response;
                } else {
                    Debug.LogError("Chat POST error: " + request.error);
                    data = $"{characterName} is asleep right now. Please come back later!";
                }
            }
            onComplete?.Invoke(data);
        }

        public string GetData() {
            return data;
        }
    }
}
