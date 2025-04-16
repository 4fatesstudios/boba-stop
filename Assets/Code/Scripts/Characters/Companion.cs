using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BobaStop.Systems;
using BobaStop.Systems.DataManagement;

namespace BobaStop.Characters
{
    public class Companion : NPC
    {
        private CompanionDataManager companionDataManager;
        
        protected override void Start() {
            base.Start(); 
            companionDataManager = new CompanionDataManager();
        }

        public CompanionDataManager GetCompanionDataManager() {
            return companionDataManager;
        }
        
        public override void Interact() {
            GameManager.Instance.dialogueManager.InitiateDialogue(npcDialogueData);
        }

        public override string GetInteractText() {
            throw new System.NotImplementedException();
        }
    }
}
