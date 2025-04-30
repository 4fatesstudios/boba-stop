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
        }

        public virtual void Interact() {
            InvokeOnInteract();
            GameManager.Instance.dialogueManager.InitiateDialogue(npcDialogueData);
        }

        protected void InvokeOnInteract() {
            OnNPCInteract?.Invoke(this, e: new OnNPCInteractArgs { transform = transform });
        }
        
        public virtual string GetInteractText() {
            throw new System.NotImplementedException();
        }
        
        public Transform GetTransform() {
            return gameObject.transform;
        }
    }
}
