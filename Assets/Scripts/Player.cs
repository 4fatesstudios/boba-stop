using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.2f;
    private SpriteRenderer spriteRenderer;
    private bool isWalking;

    private void Awake() {
        spriteRenderer = transform.Find("PlayerVisual").GetComponent<SpriteRenderer>();

        if (spriteRenderer == null) {
            Debug.LogError("SpriteRenderer not found!");
        }
    }

    private void Update() {
        Vector2 inputVector = new Vector2(0, 0);
        
        if (Input.GetKey(KeyCode.W)) {
            inputVector.y = 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.A)) {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.D)) {
            inputVector.x = 1;
        }
        
        inputVector = inputVector.normalized;
        
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        isWalking = moveDir != Vector3.zero;
        
        // Set sprite orientation to face direction moving towards
        spriteRenderer.flipX = inputVector.x == 0 ? spriteRenderer.flipX : inputVector.x < 0;
    }

    public bool IsWalking() {
        return isWalking;
    }
}
