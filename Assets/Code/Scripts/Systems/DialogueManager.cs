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
        
        public event EventHandler<OnDoDialogueEventArgs> OnDoDialogue;

        public class OnDoDialogueEventArgs : EventArgs {
            public NPCDialogueData dialogueData = null;
            public bool isInitiatingDialogue = false;
            public bool isEndingDialogue = false;
        }

        public void Start() {
            GameUIManager.Instance.dialogueUIManager.OnDialogueInput += OnInputPlayerDialogue;
            GameInput.Instance.OnEndDialogueAction += OnPlayerEndDialogue;
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

        public void InitiateDialogue(NPCDialogueData dialogueData, bool useEnvironmentContext=false) {
            GameManager.Instance.PauseDay();
            GameInput.Instance.EnableInputMapOnly(ActionMap.Dialogue);
            string[] environmentContext = GameManager.Instance.levelManagerHelper.GetCurrentLevelProperties().levelContext;
            lines = new[] {
                "Hey there friend!",
                "How's it going this fine afternoon? This dialogue is pregenerated so don't expect anything cool!",
                "Don't worry, soon we will have actual AI generated stuff!"
            };
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
                "Leaving so soon?",
                "No problem, okay bye!"
            };
            OnDoDialogue?.Invoke(this, new OnDoDialogueEventArgs {
                isEndingDialogue = true
            });
        }
    }
}
