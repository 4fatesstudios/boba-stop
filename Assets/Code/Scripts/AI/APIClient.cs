using System;
using System.Collections;
using BobaStop.Characters;
using BobaStop.NPCs;
using BobaStop.Context;
using UnityEngine;
using UnityEngine.Networking;
using System.Reflection;


namespace BobaStop.AI {
    [System.Serializable]
    public class ChatPrompt {
        public string text;
    }

    [System.Serializable]
    public class PostData {
        public string title;
        public int n;
    }

    public class RateConversationData {
        public string character_name;
    }

    [System.Serializable]
    public class CombinedData {
        public string name;
        public string age;
        public string role;
        public string living_condition;
        public string personality;
        public string beliefs;
        public string speaking_style;
        public string knowledge_scope;
        public string backstory;
        public string memory;
        public string location_knowledge;
        public string rapport_level;
        public string world_location;
        public string world_time;
        public string world_weather;
        public string level_context;
        public string current_context;
        public string player_prompt;
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

        private string getStartContext(string characterName, string playerName) {
            if (characterName == "Karen") {
                return $"you just got her drink at the {playerName}'s boba shop. You think that {playerName} got your order wrong by adding boba and you are not happy about it";
            } else if (characterName == "Jade") {
                return $"Jade is meeting the player for the first time outside the {playerName}’s boba shop. Jade is very happy with her drink and wants to tell the {playerName}. She does not know that the {playerName} is the owner of the boba shop";
            } else if (characterName == "Kaden") {
                return $"Kaden and his friends are playing a game of frisbee in the town square. He’ll see the {playerName} and wants to invite them to join in.";
            } else if (characterName == "Aster") {
                return $"{playerName} is rushing home and bumps into Aster in the town square. Aster is annoyed, but amused by {playerName} acting flustered.";
            }
            return $"you are at world Location. You ran into {playerName} there";
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
            StartCoroutine(GetFirstMeetingIEnumerator(characterData, npcData, onComplete));
        }

        private IEnumerator GetFirstMeetingIEnumerator(CompanionData characterData, NPCDialogueData npcData, Action<string, string> onComplete) {
            string url = $"{baseUrl}/first_meeting/character/{Player.Instance.GetPlayerDataManager().GetPlayerName()}";

            CombinedData requestData = new CombinedData {
                name = characterData.companionName,
                age = npcData.age,
                role = npcData.role,
                living_condition = npcData.livingCondition,
                personality = npcData.personality,
                beliefs = npcData.beliefs,
                speaking_style = npcData.speakingStyle,
                knowledge_scope = npcData.knowledgeScope,
                backstory = npcData.backstory,
                memory = npcData.memory,
                location_knowledge = "The local boba shop in town who's owner just retired.",
                rapport_level = characterData.rapportLevel.ToString(),
                // locationKnowledge = npcData.locationKnowledge[WorldContextManager.GetCurrentLevelAreaName()],
                world_location = WorldContextManager.GetCurrentLevelAreaName(),
                world_time = WorldContextManager.GetTime(),
                world_weather = WorldContextManager.GetWeather(),
                level_context = WorldContextManager.GetCurrentLevelContext()[0],
                current_context = getStartContext(characterData.companionName, Player.Instance.GetPlayerDataManager().GetPlayerName()),
                player_prompt = WorldContextManager.GetCurrentLevelContext()[1]
            };

            string json = JsonUtility.ToJson(requestData);
            Debug.Log("JSON Sent: " + json);

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
            string playerName = Player.Instance.GetPlayerDataManager().GetPlayerName();
            string url = $"{baseUrl}/new_conversation/character/{Player.Instance.GetPlayerDataManager().GetPlayerName()}";

            CombinedData requestData = new CombinedData {
                name = characterData.companionName,
                age = npcData.age,
                role = npcData.role,
                living_condition = npcData.livingCondition,
                personality = npcData.personality,
                beliefs = npcData.beliefs,
                speaking_style = npcData.speakingStyle,
                knowledge_scope = npcData.knowledgeScope,
                backstory = npcData.backstory,
                memory = npcData.memory,
                location_knowledge = "This is a new Boba Shop in town.",
                rapport_level = characterData.rapportLevel.ToString(),
                // locationKnowledge = npcData.locationKnowledge[WorldContextManager.GetCurrentLevelAreaName()],
                world_location = WorldContextManager.GetCurrentLevelAreaName(),
                world_time = WorldContextManager.GetTime(),
                world_weather = WorldContextManager.GetWeather(),
                level_context = WorldContextManager.GetCurrentLevelContext()[0], 
                current_context = $"you are at World Location. You ran into {Player.Instance.GetPlayerDataManager().GetPlayerName()} there",
                player_prompt = WorldContextManager.GetCurrentLevelContext()[1]
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
        public void SendSummarizeChat(CompanionData characterData, Action<string> onComplete) {
            StartCoroutine(SendSummarizeChatIEnumerator(characterData, onComplete));
        }

        private IEnumerator SendSummarizeChatIEnumerator(CompanionData characterData, Action<string> onComplete) {
            string url = $"{baseUrl}/summarize_chat/player/{characterData.companionName}";

            Debug.Log("Sending chat message to: " + url);

            using (UnityWebRequest request = new UnityWebRequest(url, "GET")) {
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success) {
                    string jsonResponse = request.downloadHandler.text;
                    ConversationResponse response = JsonUtility.FromJson<ConversationResponse>(jsonResponse);
                    Debug.Log("Summarized chat: " + response.response);
                    data = response.response;
                } else {
                    Debug.LogError("Chat POST error: " + request.error);
                    data = $"{characterData.companionName} is asleep right now. Please come back later!";
                }
            }
            onComplete?.Invoke(data);
        }

        // POST: "/rate_conversation/player/{player}"
        public void RateConversation(CompanionData characterData,  Action<string> onComplete) {
            StartCoroutine(RateConversationIEnumerator(characterData, onComplete));
        }
        private IEnumerator RateConversationIEnumerator(CompanionData characterData,  Action<string> onComplete) {
            string playerName = Player.Instance.GetPlayerDataManager().GetPlayerName();
            string url = $"{baseUrl}/rate_conversation/player/{playerName}";

            RateConversationData requestData = new RateConversationData {
                character_name = characterData.companionName
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
                    Debug.Log("Rating: " + response.response);
                    data = response.response;
                } else {
                    Debug.LogError("POST error: " + request.error);
                    data = $"Conversation for {characterData.companionName} had no rating";
                }
                // Handle the response as needed
                onComplete?.Invoke(data);
            }
        }

        public string GetData() {
            return data;
        }
    }
}
