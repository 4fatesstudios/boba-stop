using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.NPCs;
using BobaStop.UI;
using UnityEngine;

namespace BobaStop.Systems
{
    public class DialogueManager {
        private NPCDialogueData dialogueData;
        private string[] lines;
        
        public event EventHandler<OnInitiateDialogueEventArgs> OnDoDialogue;

        public class OnInitiateDialogueEventArgs : EventArgs {
            public NPCDialogueData dialogueData = null;
            public bool isInitiatingDialogue = false;
            public bool isEndingDialogue = false;
        }

        public void Start() {
            GameUIManager.Instance.dialogueUIManager.OnDialogueInput += OnInputPlayerDialogue;
            
            lines = new[] {
                "hey there partner!",
                "i dont like you!!!!",
                "just kidding lmao"
            };
        }

        private void OnInputPlayerDialogue(object sender, DialogueUIManager.OnDialogueInputEventArgs e) {
            // interpret player dialogue with AI
            // access string input with "e.dialogueInput"
            // somehow determine if end dialogue...
            // either call ContinueDialogue() or EndDialogue()
        }

        public void SetDialogueData(NPCDialogueData dialogueData) {
            this.dialogueData = dialogueData;
        }

        public NPCDialogueData GetDialogueData() {
            return dialogueData;
        }

        private void PopulateLines() {
            
        }

        public string[] GetLines() {
            return lines;
        }

        public void SetLines(string[] lines) {
            this.lines = lines;
        }

        public void InitiateDialogue(NPCDialogueData dialogueData) {
            this.dialogueData = dialogueData;
            OnDoDialogue?.Invoke(this, new OnInitiateDialogueEventArgs {
                dialogueData = this.dialogueData,
                isInitiatingDialogue = true
            });
        }

        public void ContinueDialogue() {
            OnDoDialogue?.Invoke(this, new OnInitiateDialogueEventArgs());
        }

        public void EndDialogue() {
            OnDoDialogue?.Invoke(this, new OnInitiateDialogueEventArgs {
                isEndingDialogue = true
            });
        }
    }
}
