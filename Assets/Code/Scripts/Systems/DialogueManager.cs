using System.Collections;
using System.Collections.Generic;
using BobaStop.NPCs;
using UnityEngine;

namespace BobaStop.Systems
{
    public class DialogueManager {
        private NPCDialogueData dialogueData;
        private string[] lines;
        private int index;

        public void Start() {
            index = 0;
            lines = new[] {
                "hey there partner!",
                "i dont like you!!!!",
                "just kidding lmao"
            };
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
    }
}
