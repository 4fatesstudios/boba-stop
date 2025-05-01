using System;
using UnityEngine;
using BobaStop.Systems;
using BobaStop.Systems.DataManagement;

namespace BobaStop.Characters
{
    public class Companion : NPC
    {
        [SerializeField] private string companionName; // gets necessary data
        private CompanionDataManager companionDataManager;

        public static event EventHandler OnCompanionInitialized;

        public static event EventHandler OnCompanionInteract;
        
        protected override void Start() {
            base.Start();
            companionDataManager = GameManager.Instance.companionManager.GetCompanionDataManager(companionName);
            
            OnCompanionInitialized?.Invoke(this, EventArgs.Empty);
        }

        public CompanionDataManager GetCompanionDataManager() {
            return companionDataManager;
        }
        
        public override void Interact() {
            InvokeOnInteract();
            OnCompanionInteract?.Invoke(this, EventArgs.Empty);
            GameManager.Instance.dialogueManager.InitiateDialogue(npcDialogueData, companionDataManager, true);
        }

        public string GetCompanionName() {
            return companionName;
        }

        public override string GetInteractText() {
            throw new System.NotImplementedException();
        }
    }
}
