using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Interactions {
    public class Interactable : MonoBehaviour, IInteractable {
        
        [SerializeField] private string interactText;
        public virtual void Interact() {
            Debug.Log("picking up interactable");
        }

        public virtual string GetInteractText() {
            return interactText;
        }

        public Transform GetTransform() {
            return transform;
        }
    }
}