using System;
using System.Collections;
using System.Collections.Generic;
using SimpleCombat;
using SimpleCombat.Components;
using UnityEngine;

public class Player : Character
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedInteractableChangedEventArgs> OnSelectedInteractableChanged;

    public class OnSelectedInteractableChangedEventArgs : EventArgs {
        public IInteractable selectedInteractable;
    }
    
    [SerializeField] private float moveSpeed = 1.2f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private GameObject attackGameObject;
    private Attack attack; // TESTING ONLY, DELETE LATER
    
    
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

        attack = attackGameObject.GetComponent<Attack>();
    }
    
    protected override void Start() {
        base.Start();
        
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void Update() {
        HandleMovement();
        HandleInteractions();
        HandleCombat();

        // quit application
        if (Input.GetKeyDown("escape")) {
            Quit();
        }
    }

    private void Quit() {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        selectedInteractable?.Interact();
    }

    private void HandleCombat() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            combatController.Attack(attack);
            base.OnAttack();
            gameInput.DisableAllInputs();
        }
    }

    public override void OnAttackAnimationFinish() {
        gameInput.EnableAllInputs();
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
        FlipSpriteToForwardVector(inputVector);
    }

    private void SetSelectedInteractable(IInteractable interactable) {
        this.selectedInteractable = interactable;
        
        OnSelectedInteractableChanged?.Invoke(this, new OnSelectedInteractableChangedEventArgs {
            selectedInteractable = selectedInteractable
        });
    }
    
}
