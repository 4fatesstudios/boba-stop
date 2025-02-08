using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHouseInteract : MonoBehaviour, IInteractable
{
    public void Interact() {
        Debug.Log("Entering shop!");
    }
    public string GetInteractText() {
        return "Enter Shop";
    }
}
