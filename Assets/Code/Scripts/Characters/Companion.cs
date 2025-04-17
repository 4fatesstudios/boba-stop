using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BobaStop.Systems;
using BobaStop.Systems.DataManagement;

namespace BobaStop.Characters
{
    public class Companion : NPC
    {
        [SerializeField] private string companionName; // gets necessary data
        private CompanionDataManager companionDataManager;
        
        protected override void Start() {
            base.Start();
            companionDataManager = GameManager.Instance.companionManager.GetCompanionDataManager(companionName);
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
