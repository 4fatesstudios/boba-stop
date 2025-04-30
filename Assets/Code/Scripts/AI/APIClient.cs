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
    }

    [System.Serializable]
    public class ConversationResponse {
        public string response;
        public string goodbye = "error";
    }

    public class APIClient : MonoBehaviour {
        [Header("FastAPI Settings")]
        public string baseUrl = "http://127.0.0.1:8000";

        [Header("Test Options")]
        public bool callFirstMeetingOnStart = true;
        public bool callNewConversationOnStart = false;

        private string data;

        private string goodbye;

        void Start() {    
        }

        // GET: /first_meeting/character/{character_name}
        public void GetFirstMeeting(CompanionData characterData, NPCDialogueData npcData, Action<string, string> onComplete) {
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
                locationKnowledge = WorldContextManager.GetCurrentLevelContext()[0], 
                worldLocation = WorldContextManager.GetCurrentLevelAreaName(),
                worldTime = WorldContextManager.GetTime(),
                worldWeather = WorldContextManager.GetWeather()
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

        public void SendNewConversation(string characterName, int rapportLevel, int rapportLevelProgress, string currentStory, Action<string, string> onComplete) {
            StartCoroutine(SendNewConversationIEnumerator(characterName, rapportLevel, rapportLevelProgress, currentStory, onComplete));
        }
        private IEnumerator SendNewConversationIEnumerator(string characterName, int rapportLevel, int rapportLevelProgress, string currentStory, Action<string, string> onComplete) {
            string url = $"{baseUrl}/new_conversation/character/{characterName}";

            ConversationRequest requestData = new ConversationRequest {
                rapport_level = rapportLevel,
                rapport_level_progress = rapportLevelProgress,
                current_story = currentStory
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
                    data = $"{characterName} is asleep right now. Please come back later!";
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
        public void SendSummarizeChat(string characterName) {
            StartCoroutine(SendSummarizeChatIEnumerator(characterName));
        }

        private IEnumerator SendSummarizeChatIEnumerator(string characterName) {
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
        }

        public string GetData() {
            return data;
        }
    }
}
