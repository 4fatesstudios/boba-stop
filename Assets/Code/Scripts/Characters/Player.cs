using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.Data.Saved;
using BobaStop.Systems;
using SimpleCombat;
using SimpleCombat.Components;
using UnityEngine;

namespace BobaStop.Characters {
    public class Player : Character {
        public static Player Instance { get; private set; }

        public event EventHandler<OnSelectedInteractableChangedEventArgs> OnSelectedInteractableChanged;
        
        public class OnSelectedInteractableChangedEventArgs : EventArgs {
            public Interactions.IInteractable selectedInteractable;
        }

        [SerializeField] private float moveSpeed = 1.2f;
        [SerializeField] private Systems.GameInput gameInput;
        [SerializeField] private GameObject attackGameObject;
        private Attack attack; // TESTING ONLY, DELETE LATER

        private CharacterController controller;

        private readonly float interactDistance = 0.5f;
        private Vector3 lastInteractDir;
        private Interactions.IInteractable selectedInteractable;

        private void Awake() {
            if (Instance != null) {
                Debug.LogError($"{nameof(Player)} already exists.");
            }

            Instance = this;

            attack = attackGameObject.GetComponent<Attack>();
            controller = GetComponent<CharacterController>();
        }

        protected override void Start() {
            base.Start();

            gameInput.OnInteractAction += GameInput_OnInteractAction;
            gameInput.OnAttackAction += GameInput_OnAttackAction;
        }

        private void Update() {
            HandleMovement();
            HandleInteractions();

            // quit application
            if (Input.GetKeyDown("escape")) {
                Quit();
            }

            if (Input.GetKeyDown(KeyCode.O)) {
                Debug.Log(GameManager.Instance.dayCycleManager.GetStandardTime());
            }
            if (Input.GetKeyDown(KeyCode.P)) {
                Debug.Log(GameManager.Instance.dayCycleManager.GetMilitaryTime());
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                GameManager.Instance.shopManager.PrintShopSelection();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                GameManager.Instance.shopManager.AddToShopSelection(Resources.Load<Items.Resource>("Items/Resource/Bases/GreenTea"));
                GameManager.Instance.shopManager.AddToShopSelection(Resources.Load<Items.Resource>("Items/Resource/Toppings/TestTopping1"));
                GameManager.Instance.shopManager.AddToShopSelection(Resources.Load<Items.Resource>("Items/Resource/Toppings/TestTopping2"));
            }

            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                GameManager.Instance.shopManager.RemoveFromShopSelection(Resources.Load<Items.Resource>("Items/Resource/Bases/GreenTea"));
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                GameManager.Instance.shopManager.RemoveFromShopSelection(Resources.Load<Items.Resource>("Items/Resource/Bases/BlackTea"));
            }

            if (Input.GetKeyDown(KeyCode.Alpha5)) {
                GameManager.Instance.shopManager.GenerateOrder();
            }

            if (Input.GetKeyDown(KeyCode.Alpha6)) {
                GameManager.Instance.shopManager.GetShopSelectionScore();
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

        private void GameInput_OnAttackAction(object sender, EventArgs e) {
            HandleCombat();
        }

        private void HandleCombat() {
            base.OnAttack();
            gameInput.DisableAllInputs();
        }

        public override void OnAttack() {
            combatController.Attack(attack);
        }

        public override void OnAttackFinish() {
            gameInput.EnableAllInputs();
        }

        private void HandleInteractions() {
            List<Interactions.IInteractable> interactableList = new List<Interactions.IInteractable>();
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactDistance);
            foreach (Collider collider in colliderArray) {
                if (collider.TryGetComponent(out Interactions.IInteractable interactable)) {
                    interactableList.Add(interactable);
                }
            }

            selectedInteractable = null;
            foreach (Interactions.IInteractable interactable in interactableList) {
                if (selectedInteractable == null) {
                    selectedInteractable = interactable;
                }
                else {
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
            Vector3 moveDir = new Vector3(inputVector.x * moveSpeed, controller.isGrounded ? 0.0f : -Gravity,
                inputVector.y * moveSpeed);

            controller.Move(moveDir * Time.deltaTime);

            isWalking = moveDir.x != 0 || moveDir.z != 0;

            // Set sprite orientation to face direction moving towards
            FlipSpriteToForwardVector(inputVector);
        }

        private void SetSelectedInteractable(Interactions.IInteractable interactable) {
            this.selectedInteractable = interactable;

            OnSelectedInteractableChanged?.Invoke(this, new OnSelectedInteractableChangedEventArgs {
                selectedInteractable = selectedInteractable
            });
        }

    }
}