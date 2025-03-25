using System.Collections;
using System.Collections.Generic;
using BobaStop.NPCs;
using UnityEngine;

namespace BobaStop.Systems
{
    public class DialogueManager {
        private NPCDialogueData dialogueData;
        private int index;

        public void Start() {
            index = 0;
        }

        public void SetDialogueData(NPCDialogueData dialogueData) {
            this.dialogueData = dialogueData;
        }

        public NPCDialogueData GetDialogueData() {
            return dialogueData;
        }
    }
}
