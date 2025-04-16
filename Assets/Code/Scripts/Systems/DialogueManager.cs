using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BobaStop.AI;
using BobaStop.NPCs;
using BobaStop.UI;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace BobaStop.Systems
{
    public class DialogueManager {
        public static DialogueManager Instance { get; private set; }
        private NPCDialogueData dialogueData;
        private string[] lines;

        private int maxCharPerLine = 100;

        private CompanionData companionData;
        
        public event EventHandler<OnDoDialogueEventArgs> OnDoDialogue;
        [SerializeField] private GameObject apiClientComponent;
        private APIClient apiClient;

        private string dialogueText;

        public class OnDoDialogueEventArgs : EventArgs {
            public NPCDialogueData dialogueData = null;
            public bool isInitiatingDialogue = false;
            public bool isEndingDialogue = false;
        }

        public void Start() {
            if (apiClientComponent == null) {
                apiClientComponent = new GameObject("APIClientObject");
            }
            UnityEngine.Object.DontDestroyOnLoad(apiClientComponent); // 👈 this line is key!

            GameUIManager.Instance.dialogueUIManager.OnDialogueInput += OnInputPlayerDialogue;
            GameInput.Instance.OnEndDialogueAction += OnPlayerEndDialogue;

            companionData = ScriptableObject.CreateInstance<CompanionData>();
            companionData.companionName = "Karen";
            companionData.rapportLevel = RapportLevel.Neutral;
            companionData.rapportLevelProgress = 0;

            apiClientComponent.AddComponent<APIClient>();
            apiClient = apiClientComponent.GetComponent<APIClient>();
            
            apiClient.GetFirstMeeting(companionData.companionName, ReturnData);
            Debug.Log("First meeting called in DialogueManager");
        }

        private void OnInputPlayerDialogue(object sender, DialogueUIManager.OnDialogueInputEventArgs e) {
            // interpret player dialogue with AI
            // access string input with "e.dialogueInput"
            // somehow determine if end dialogue...
            // either call ContinueDialogue() or EndDialogue()
            if (e.dialogueInput == "Goodbye!") {
                EndDialogue();
                return;
            }
            apiClient.SendChatMessage(companionData.companionName, e.dialogueInput, ReturnData);
            Debug.Log("Received player input in DialogueManager: " + e.dialogueInput);
        }

        public void SetDialogueData(NPCDialogueData dialogueData) {
            this.dialogueData = dialogueData;
        }

        public NPCDialogueData GetDialogueData() {
            return dialogueData;
        }

        private void ReturnData(string response){
            Debug.Log("Received first meeting in DialogueManager: " + response);
            dialogueText = response;
            PopulateLines();
        }

        private void PopulateLines() {
            if (dialogueText == null) {
                lines = new[] {
                    "Hey there friend!",
                    "How's it going this fine afternoon? This dialogue is pregenerated so don't expect anything cool!",
                    "Don't worry, soon we will have actual AI generated stuff!"
                };
            } else {
                // Split the dialogue text into lines based on the max character limit
                Populate();
                ContinueDialogue();
            }
        }

        private void Populate() {
            List<string> lineList = new List<string>();
            string[] words = dialogueText.Split(' ');
            string currentLine = "";
            foreach (string word in words) {
                if (currentLine.Length + word.Length + 1 <= maxCharPerLine) {
                    currentLine += (currentLine.Length > 0 ? " " : "") + word;
                } else {
                    lineList.Add(currentLine);
                    currentLine = word;
                }
            }
            if (currentLine.Length > 0) {
                lineList.Add(currentLine);
            }
            lines = lineList.ToArray();
        }

        public string[] GetLines() {
            return lines;
        }

        public void SetLines(string[] lines) {
            this.lines = lines;
        }

        public void InitiateDialogue(NPCDialogueData dialogueData) {
            GameInput.Instance.EnableInputMapOnly(ActionMap.Dialogue);
            lines = new[] {
                "Hey there!"
            };
            Populate();
            this.dialogueData = dialogueData;
            OnDoDialogue?.Invoke(this, new OnDoDialogueEventArgs {
                dialogueData = this.dialogueData,
                isInitiatingDialogue = true
            });
        }

        public void ContinueDialogue() {
            OnDoDialogue?.Invoke(this, new OnDoDialogueEventArgs());
        }

        private void OnPlayerEndDialogue(object sender, EventArgs e) {
            EndDialogue();
        }

        public void EndDialogue() {
            lines = new[] {
                "Goodbye"
            };
            OnDoDialogue?.Invoke(this, new OnDoDialogueEventArgs {
                isEndingDialogue = true
            });
        }
    }
}
