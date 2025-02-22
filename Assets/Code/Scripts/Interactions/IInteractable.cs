using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Interactions {
    public interface IInteractable {
        void Interact();
        string GetInteractText();
        Transform GetTransform();
    }
}