using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.Interactions;
using BobaStop.NPCs;
using BobaStop.Systems;
using UnityEngine;

namespace BobaStop.Characters
{
    public class NPC : Character, IInteractable {
        [SerializeField] protected NPCDialogueData npcDialogueData;

        public event EventHandler OnInteract;
        
        protected override void Start() {
            base.Start(); 
        }

        public virtual void Interact() {
            OnInteract?.Invoke(this, EventArgs.Empty);
            GameManager.Instance.dialogueManager.InitiateDialogue(npcDialogueData);
        }
        
        public virtual string GetInteractText() {
            throw new System.NotImplementedException();
        }
        
        public Transform GetTransform() {
            return gameObject.transform;
        }
    }
}
