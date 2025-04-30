using System;
using System.Collections.Generic;
using System.Linq;
using BobaStop.AI;
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

        private int maxCharPerLine = 100;

        private CompanionData companionData;
        
        public event EventHandler<OnDoDialogueEventArgs> OnDoDialogue;
        public event EventHandler OnInappropriateInput;
        
        [SerializeField] private GameObject apiClientComponent;
        private APIClient apiClient;

        private string dialogueText;

        public class OnDoDialogueEventArgs : EventArgs {
            public CompanionDataManager companionDataManager = null;
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
            
            companionData = new CompanionData {
                companionName = "Karen",
                rapportLevel = RapportLevel.Neutral,
                rapportLevelProgress = 0
            };

            apiClientComponent.AddComponent<APIClient>();
            apiClient = apiClientComponent.GetComponent<APIClient>();

            apiClient.GetFirstMeeting(companionData, dialogueData, ReturnData);
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

            if (!InputFilter.IsInputAllowed(e.dialogueInput)) {
                OnInappropriateInput?.Invoke(this, e);
                Debug.Log("Inappropriate input detected");
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

        public void InitiateDialogue(NPCDialogueData dialogueData, CompanionDataManager companionDataManager=null, bool useEnvironmentContext=false) {
            if (dialogueData == null) {
                Debug.LogError("Dialogue Data is null");
                return;
            }
            
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
            lines = new[] {
                "Goodbye"
            };
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

