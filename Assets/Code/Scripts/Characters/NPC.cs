using System.Collections;
using System.Collections.Generic;
using BobaStop.Interactions;
using BobaStop.NPCs;
using BobaStop.Systems;
using UnityEngine;

namespace BobaStop.Characters
{
    public class NPC : Character, IInteractable {
        [SerializeField] private NPCDialogueData npcDialogueData;
        
        public void Interact() {
            GameManager.Instance.dialogueManager.InitiateDialogue(npcDialogueData);
        }
        
        public string GetInteractText() {
            throw new System.NotImplementedException();
        }
        
        public Transform GetTransform() {
            return gameObject.transform;
        }
    }
}
