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

        protected override void Start() {
            base.Start(); 
        }

        public virtual void Interact() {
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
