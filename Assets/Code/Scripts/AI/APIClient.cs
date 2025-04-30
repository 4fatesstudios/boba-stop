using System;
using System.Collections;
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

    [System.Serializable]
    public class ConversationResponse {
        public string response;
    }

    public class APIClient : MonoBehaviour {
        public static APIClient Instance { get; private set; }
        
        [Header("FastAPI Settings")]
        public string baseUrl = "http://127.0.0.1:8000";

        [Header("Test Options")]
        public bool callFirstMeetingOnStart = true;
        public bool callNewConversationOnStart = false;

        private string data;

        public bool IsReady { get; private set; } = false;
        public float startupTimeout = 60f; // configurable in Inspector
        public string pingEndpoint = "/health"; // your FastAPI server should expose this
        
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
            DontDestroyOnLoad(this);
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
        public void GetFirstMeeting(string characterName, Action<string> onComplete) {
            StartCoroutine(GetFirstMeetingIEnumerator(characterName, onComplete));
        }

        private IEnumerator GetFirstMeetingIEnumerator(string characterName, Action<string> onComplete) {
            string url = $"{baseUrl}/first_meeting/character/{characterName}";

            using (UnityWebRequest request = UnityWebRequest.Get(url)) {
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success) {
                    string jsonResponse = request.downloadHandler.text;
                    ConversationResponse response = JsonUtility.FromJson<ConversationResponse>(jsonResponse);
                    Debug.Log("First meeting response: " + response.response);
                    data = response.response;
                } else {
                    Debug.LogError("GET error: " + request.error);
                    data = $"{characterName} is asleep right now. Please come back later!";
                }
            }
            onComplete?.Invoke(data);
        }

        // POST: /new_conversation/character/{character_name}

        public void SendNewConversation(string characterName, int rapportLevel, int rapportLevelProgress, string currentStory, Action<string> onComplete) {
            StartCoroutine(SendNewConversationIEnumerator(characterName, rapportLevel, rapportLevelProgress, currentStory, onComplete));
        }
        private IEnumerator SendNewConversationIEnumerator(string characterName, int rapportLevel, int rapportLevelProgress, string currentStory, Action<string> onComplete) {
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
                } else {
                    Debug.LogError("POST error: " + request.error);
                    data = $"{characterName} is asleep right now. Please come back later!";
                }
                // Handle the response as needed
                onComplete?.Invoke(data);
            }
        }

        // POST: /chat/character/{character_name}
        public void SendChatMessage(string characterName, string prompt, Action<string> onComplete) {
            StartCoroutine(SendChatMessageIEnumerator(characterName, prompt, onComplete));
        }
        private IEnumerator SendChatMessageIEnumerator(string characterName, string prompt, Action<string> onComplete) {
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
                } else {
                    Debug.LogError("Chat POST error: " + request.error);
                    data = $"{characterName} is asleep right now. Please come back later!";
                }
            }

            onComplete?.Invoke(data);
        }

        // POST: /summarize_chat/character/{character}
        // Not tested yet
        public void SendSummarizeChat(string characterName, string chatHistory, Action<string> onComplete) {
            StartCoroutine(SendSummarizeChatIEnumerator(characterName, chatHistory, onComplete));
        }

        private IEnumerator SendSummarizeChatIEnumerator(string characterName, string chatHistory, Action<string> onComplete) {
            string url = $"{baseUrl}/summarize_chat/character/{characterName}";

            Debug.Log("Sending chat message to: " + url);
            Debug.Log("Chat History: " + chatHistory);

            ChatPrompt chatPrompt = new ChatPrompt {
                text = chatHistory
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
