using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.2f;
    [SerializeField] private GameInput gameInput;
    private SpriteRenderer spriteRenderer;
    private bool isWalking;
    private float playerRadius = 0.1f;
    private float playerHeight = 0.15f;

    private void Awake() {
        spriteRenderer = transform.Find("PlayerVisual").GetComponent<SpriteRenderer>();

        if (spriteRenderer == null) {
            Debug.LogError("SpriteRenderer not found!");
        }
    }

    private void Update() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        
        if (!canMove) {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove) {
                moveDir = moveDirX;
            }
            else {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove) {
                    moveDir = moveDirZ;
                }
                else {
                    // cannot move
                }
            }
        }
        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
        
        // Set sprite orientation to face direction moving towards
        spriteRenderer.flipX = inputVector.x == 0 ? spriteRenderer.flipX : inputVector.x < 0;
    }

    public bool IsWalking() {
        return isWalking;
    }
}
