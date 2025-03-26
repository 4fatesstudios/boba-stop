using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BobaStop.Interactions;
using BobaStop.NPCs;

namespace BobaStop.Systems
{
    public class DialogueManager : MonoBehaviour
    {
        public int maxMessages = 25;
        public GameObject chatPanel, textObject;
        public TMP_InputField chatBox;
        // Start is called before the first frame update
        [SerializeField] private CompanionData companionData;
        [SerializeField] private OllamaGenerator generator;

        [SerializeField] List<Message> messages = new List<Message>();
        void Start() {
            companionData = ScriptableObject.CreateInstance<CompanionData>();
            companionData.companionName = "Karen";
            companionData.rapportLevel = RapportLevel.Neutral;
            companionData.rapportLevelProgress = 0;
            generator = new OllamaGenerator(companionData);
            generator.Start();
        }

        // Update is called once per frame
        void Update() {
            if (chatBox.text != "") {
                if (Input.GetKeyDown(KeyCode.Return)) {
                    SendMessageToGenerator(chatBox.text);
                    chatBox.text = "";
                }
                
            }
            if (!chatBox.isFocused) {
                if (Input.GetKeyDown(KeyCode.Space)) {
                    SendMessageToChat("You pressed the space bar.");
                    Debug.Log("Space");
                }
            }  
        }

        public void SendMessageToChat(string text) {
            if (messages.Count >= maxMessages) {
                Destroy(messages[0].textObject);
                messages.RemoveAt(0);
            }

            Message newMessage = new Message();
            newMessage.text = text;

            GameObject newText = Instantiate(textObject, chatPanel.transform);
            newMessage.textObject = newText; 

            TextMeshProUGUI messageText = newText.GetComponentInChildren<TextMeshProUGUI>(); 
            if (messageText != null) {
                messageText.text = text; 
                messageText.color = Color.white;
                Debug.Log("Text successfully updated: " + text); // ✅ Debugging
            } else {
                Debug.LogError("TextMeshProUGUI component not found in instantiated object!");
            }  

            messages.Add(newMessage);
        }

        public void SendMessageToGenerator(string text) {
            SendMessageToChat(text);
            string responseText = generator.Chat(text).Result;
            SendMessageToChat(responseText);
        }

        [System.Serializable]
        public class Message {
            public string text;
            public GameObject textObject;
        }
    }
}
