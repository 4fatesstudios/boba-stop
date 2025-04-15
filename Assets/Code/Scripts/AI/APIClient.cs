using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace BobaStop.AI {
    [System.Serializable]
    public class ChatPrompt
    {
        public string text;
    }

    [System.Serializable]
    public class ConversationRequest
    {
        public int rapport_level;
        public int rapport_level_progress;
        public string current_story;
    }

    [System.Serializable]
    public class ConversationResponse
    {
        public string response;
    }

    public class APIClient : MonoBehaviour
    {
        [Header("FastAPI Settings")]
        public string baseUrl = "http://127.0.0.1:8000";

        [Header("Test Options")]
        public bool callFirstMeetingOnStart = true;
        public bool callNewConversationOnStart = false;

        private string data;

        void Start()
        {
            // if (callNewConversationOnStart)
            //     StartCoroutine(SendNewConversation());
        }

        // GET: /first_meeting/character/{character_name}
        public IEnumerator GetFirstMeeting(string characterName, Action<string> onComplete)
        {
            string url = $"{baseUrl}/first_meeting/character/{characterName}";

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    ConversationResponse response = JsonUtility.FromJson<ConversationResponse>(jsonResponse);
                    Debug.Log("First meeting response: " + response.response);
                    data = request.downloadHandler.text;
                }
                else
                {
                    Debug.LogError("GET error: " + request.error);
                    data = $"{characterName} is asleep right now. Please come back later!";
                }
            }
            onComplete?.Invoke(data);
        }

        // POST: /new_conversation/character/{character_name}
        public IEnumerator SendNewConversation(string characterName, int rapportLevel, int rapportLevelProgress, string currentStory, Action<string> onComplete)
        {
            string url = $"{baseUrl}/new_conversation/character/{characterName}";

            ConversationRequest requestData = new ConversationRequest
            {
                rapport_level = rapportLevel,
                rapport_level_progress = rapportLevelProgress,
                current_story = currentStory
            };

            string json = JsonUtility.ToJson(requestData);

            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    ConversationResponse response = JsonUtility.FromJson<ConversationResponse>(jsonResponse);
                    Debug.Log("New conversation response: " + response.response);
                    data = request.downloadHandler.text;
                }
                else
                {
                    Debug.LogError("POST error: " + request.error);
                    data = $"{characterName} is asleep right now. Please come back later!";
                }
                // Handle the response as needed
                onComplete?.Invoke(data);
            }
        }

        // POST: /chat/character/{character_name}
        public IEnumerator SendChatMessage(string characterName, string prompt, Action<string> onComplete)
        {
            string url = $"{baseUrl}/chat/character/{characterName}";

            ChatPrompt chatPrompt = new ChatPrompt
            {
                text = prompt
            };

            string json = JsonUtility.ToJson(chatPrompt);

            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    ConversationResponse response = JsonUtility.FromJson<ConversationResponse>(jsonResponse);
                    Debug.Log("Chat response: " + response.response);
                    data = request.downloadHandler.text;
                }
                else
                {
                    Debug.LogError("Chat POST error: " + request.error);
                    data = $"{characterName} is asleep right now. Please come back later!";
                }
            }

            onComplete?.Invoke(data);
        }

        public string GetData()
        {
            return data;
        }
    }
}
