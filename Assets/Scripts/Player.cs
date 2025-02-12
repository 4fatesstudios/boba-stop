using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedInteractableChangedEventArgs> OnSelectedInteractableChanged;

    public class OnSelectedInteractableChangedEventArgs : EventArgs {
        public IInteractable selectedInteractable;
    }
    
    [SerializeField] private float moveSpeed = 1.2f;
    [SerializeField] private GameInput gameInput;
    
    private SpriteRenderer spriteRenderer;
    private bool isWalking;
    private readonly float playerRadius = 0.1f;
    private readonly float playerHeight = 0.15f;
    private readonly float interactDistance = 0.5f;
    private Vector3 lastInteractDir;
    private IInteractable selectedInteractable;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError($"{nameof(Player)} already exists.");
        }
        Instance = this;
    }
    
    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        
        spriteRenderer = transform.Find("PlayerVisual").GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            Debug.LogError("SpriteRenderer not found!");
        }
    }

    private void Update() {
        HandleMovement();
        HandleInteractions();
    }
    
    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        selectedInteractable?.Interact();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleInteractions() {
        List<IInteractable> interactableList = new List<IInteractable>();
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactDistance);
        foreach (Collider collider in colliderArray) {
            if (collider.TryGetComponent(out IInteractable interactable)) {
                interactableList.Add(interactable);
            }
        }
        
        selectedInteractable = null;
        foreach (IInteractable interactable in interactableList) {
            if (selectedInteractable == null) {
                selectedInteractable = interactable;
            } else {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) <
                    Vector3.Distance(transform.position, selectedInteractable.GetTransform().position)) {
                    selectedInteractable = interactable;
                }
            }
        }
        
        SetSelectedInteractable(selectedInteractable);
    }
    
    private void HandleMovement() {
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

    private void SetSelectedInteractable(IInteractable interactable) {
        this.selectedInteractable = interactable;
        
        OnSelectedInteractableChanged?.Invoke(this, new OnSelectedInteractableChangedEventArgs {
            selectedInteractable = selectedInteractable
        });
    }
}
