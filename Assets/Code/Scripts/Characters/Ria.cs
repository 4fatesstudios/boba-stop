using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.Systems;
using UnityEngine;

namespace BobaStop.Characters
{
    public class Ria : NPC {
        public static event EventHandler OnRiaInteract;
        
        public override void Interact() {
            GameManager.Instance.dialogueManager.InitiateDialogue(npcDialogueData);
            
            OnRiaInteract?.Invoke(this, EventArgs.Empty);
        }
    }
}
