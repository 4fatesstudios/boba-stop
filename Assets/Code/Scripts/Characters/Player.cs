using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.Data;
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
        }

        private void Update() {
            HandleMovement();
            HandleInteractions();
            HandleCombat();

            // quit application
            if (Input.GetKeyDown("escape")) {
                Quit();
            }

            SaveTests();
        }

        private void SaveTests() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                GameManager.Instance.SetSaveSlot(1);
                Debug.Log("SAVE SLOT SET TO 1");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                GameManager.Instance.SetSaveSlot(2);
                Debug.Log("SAVE SLOT SET TO 2");
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                GameManager.Instance.SaveData();
                Debug.Log("SAVED");
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                GameManager.Instance.LoadData();
                Debug.Log("LOADED");
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha5)) {
                GameManager.Instance.SetPlayerName("Chris");
                Debug.Log("SET PLAYER NAME TO \"Chris\"");
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha6)) {
                GameManager.Instance.SetPlayerName("Angel");
                Debug.Log("SET PLAYER NAME TO \"Angel\"");
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha7)) {
                GameManager.Instance.SetPlayerName("Alexis");
                Debug.Log("SET PLAYER NAME TO \"Alexis\"");
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha0)) {
                GameManager.Instance.GetPlayerName();
                Debug.Log("PLAYER NAME: " + GameManager.Instance.GetPlayerName());
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
                base.OnAttack();
                gameInput.DisableAllInputs();
            }
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