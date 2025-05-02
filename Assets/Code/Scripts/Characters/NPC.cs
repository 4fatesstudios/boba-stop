using System;
using BobaStop.Interactions;
using BobaStop.NPCs;
using BobaStop.Systems;
using UnityEngine;

namespace BobaStop.Characters
{
    public class NPC : Character, IInteractable {
        [SerializeField] protected NPCDialogueData npcDialogueData;

        public static event EventHandler<OnNPCInteractArgs> OnNPCInteract;
        public static event EventHandler OnNPCInteractEnd;

        public class OnNPCInteractArgs : EventArgs {
            public Transform transform;
        }
        
        protected override void Start() {
            base.Start();

            DialogueManager.OnInitiateDialogueSuccess += InvokeOnInteract;
        }

        public virtual void Interact() {
            GameManager.Instance.dialogueManager.InitiateDialogue(npcDialogueData);
        }

        private void InvokeOnInteract(object sender, EventArgs e) {
            OnNPCInteract?.Invoke(this, e: new OnNPCInteractArgs { transform = transform });
        }
        
        public virtual string GetInteractText() {
            throw new System.NotImplementedException();
        }
        
        public Transform GetTransform() {
            return gameObject.transform;
        }

        public NPCDialogueData GetNPCDialogueData() {
            return npcDialogueData;
        }
    }
}
