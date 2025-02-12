using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTest : MonoBehaviour, IInteractable
{
    public void Interact() {
        Debug.Log("picking up sword!");
    }
    
    public string GetInteractText() {
        return "Pick Up";
    }

    public Transform GetTransform() {
        return transform;
    }
}
