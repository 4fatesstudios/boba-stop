using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BobaStop.AI;
using BobaStop.Characters;
using BobaStop.NPCs;
using BobaStop.Systems.DataManagement;
using BobaStop.UI;
using UnityEngine;

namespace BobaStop.Systems
{
    public class DialogueManager {
        private CompanionDataManager companionDataManager;
        private NPCDialogueData dialogueData;
        private string[] lines;

        private int maxCharPerLine = 200;

        private CompanionData companionData;
        
        public event EventHandler<OnDoDialogueEventArgs> OnDoDialogue;
        public event EventHandler OnInappropriateInput;
        public static event EventHandler OnCompanionStillThinking;
        public static event EventHandler OnInitiateDialogueSuccess;
        
        private APIClient apiClient;

        private string dialogueText;

        private bool isFirstMeeting = true;

        public class OnDoDialogueEventArgs : EventArgs {
            public CompanionDataManager companionDataManager = null;
            public NPCDialogueData dialogueData = null;
            public bool isInitiatingDialogue = false;
            public bool isEndingDialogue = false;
        }

        public void Start() {
            GameUIManager.Instance.dialogueUIManager.OnDialogueInput += OnInputPlayerDialogue;
            GameInput.Instance.OnEndDialogueAction += OnPlayerEndDialogue;
            Companion.OnCompanionInitialized += GenerateConversationStart;
            
            apiClient = APIClient.Instance;
        }
        
        private void GenerateConversationStart(object sender, EventArgs e) {
            var companion = sender as Companion;
            companionData = companion.GetCompanionDataManager().GetCompanionData();
            dialogueData = companion.GetNPCDialogueData();
            if (isFirstMeeting) {
                apiClient.GetFirstMeeting(companionData, dialogueData, StartData);
                Debug.Log("First meeting called in DialogueManager");
                isFirstMeeting = false;
            } else {
                apiClient.SendNewConversation(companionData, dialogueData, StartData);
                Debug.Log("New conversation called in DialogueManager");
            }
        }

        private IEnumerator AwaitAPIServer() {
            yield return new WaitUntil(() => apiClient.IsReady);
        }

        private void OnInputPlayerDialogue(object sender, DialogueUIManager.OnDialogueInputEventArgs e) {
            // interpret player dialogue with AI
            // access string input with "e.dialogueInput"
            // somehow determine if end dialogue...
            // either call ContinueDialogue() or EndDialogue()

            if (!InputFilter.IsInputAllowed(e.dialogueInput)) {
                OnInappropriateInput?.Invoke(this, e);
                Debug.Log("Inappropriate input detected");
                return;
            }
            
            apiClient.SendChatMessage(companionData.companionName, e.dialogueInput, ReturnData);
            lines = new[] {
                "Hmm..."
            };
            Debug.Log("Received player input in DialogueManager: " + e.dialogueInput);
        }

        public void SetDialogueData(NPCDialogueData dialogueData) {
            this.dialogueData = dialogueData;
        }

        public NPCDialogueData GetDialogueData() {
            return dialogueData;
        }

        private void StartData(string response, string goodbye) {
            Debug.Log("Received first meeting in DialogueManager: " + response);
            dialogueText = response;
        }

        private void ReturnData(string response, string goodbye){
            if (goodbye == "error") {
                Debug.LogError("Error in API response");
                return;
            }
            Debug.Log("Received first meeting in DialogueManager: " + response);
            dialogueText = response;
            PopulateLines(goodbye);
        }

        private void AddMemory(string memory) {
            if (memory == "error") {
                Debug.LogError("Error in API response");
                return;
            }
            dialogueData.memory += memory;
            dialogueData.memory += ",\n";
            Debug.Log("Received memory in DialogueManager: " + memory);
        }

        private void PopulateLines(string goodbye) {
            if (dialogueText == null) {
                lines = new[] {
                    "Hey there friend!",
                    "How's it going this fine afternoon? This dialogue is pregenerated so don't expect anything cool!",
                    "Don't worry, soon we will have actual AI generated stuff!"
                };
            } else {
                // Split the dialogue text into lines based on the max character limit
                Populate();
                if (goodbye == "true") {
                    EndDialogue();
                } else {
                    ContinueDialogue();
                }   
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

        public void InitiateDialogue(NPCDialogueData dialogueData, CompanionDataManager companionDataManager=null, bool useEnvironmentContext=false) {
            if (string.IsNullOrEmpty(dialogueText)) {
                Debug.Log("Still thinking!!!");
                OnCompanionStillThinking?.Invoke(this, EventArgs.Empty);
                return;
            }
            
            if (dialogueData == null) {
                Debug.LogError("Dialogue Data is null");
                return;
            }
            
            OnInitiateDialogueSuccess?.Invoke(this, EventArgs.Empty);
            
            GameManager.Instance.PauseDay();
            GameInput.Instance.EnableInputMapOnly(ActionMap.Dialogue);
            string[] environmentContext = GameManager.Instance.levelManagerHelper.GetCurrentLevelProperties().levelContext;
            lines = new[] {
                "Hey there!"
            };
            Populate();
            this.companionDataManager = companionDataManager;
            this.dialogueData = dialogueData;
            OnDoDialogue?.Invoke(this, new OnDoDialogueEventArgs {
                companionDataManager = this.companionDataManager,
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
            apiClient.SendSummarizeChat(companionData.companionName, AddMemory);
            OnDoDialogue?.Invoke(this, new OnDoDialogueEventArgs {
                isEndingDialogue = true
            });
        }
    }
    
    public class InputFilter {
        private static readonly List<string> bannedWords = new List<string>
        {
            // AI/OOC breaking
            "you are an ai", "you are not real", "chatgpt", "language model", "ollama",
            "stay in character", "break character", "ignore previous", "jailbreak", "system prompt",

            // Worldbreaking
            "what year is it", "summarize", "describe yourself", "make a list", "switch personality",
            "who created you", "act like another character", "training data",

            // NSFW
            "sex", "sexual", "nude", "naked", "fetish", "erotic", "orgasm", "cum", "moan",
            "wet", "explicit", "onlyfans", "porn", "blowjob", "fuck", "pussy", "cock",
            "anal", "threesome", "rape", "incest", "slave",

            // Dangerous/Illegal
            "how to make a bomb", "how to kill", "how to hack", "school shooting",
            "murder", "torture", "self harm", "suicide", "i want to die"
        };

        public static bool IsInputAllowed(string input)
        {
            string lowerInput = input.ToLower();

            return !bannedWords.Any(banned => lowerInput.Contains(banned));
        }
    }
}

