using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    private void Update() {
        if (Input.GetKey(KeyCode.W)) {
            Debug.Log("W");
        }
        else {
            Debug.Log("-");
        }
    }
}
