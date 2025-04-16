using System;
using System.Collections.Generic;
using BobaStop.Systems;
using BobaStop.Systems.DataManagement;
using SimpleCombat.Components;
using UnityEngine;
using UnityEngine.UIElements;

namespace BobaStop.Characters {
    public class Player : CombatCharacter {
        public static Player Instance { get; private set; }

        public event EventHandler<OnSelectedInteractableChangedEventArgs> OnSelectedInteractableChanged;
        
        public event EventHandler OnOpenInventory;
        
        public class OnSelectedInteractableChangedEventArgs : EventArgs {
            public Interactions.IInteractable selectedInteractable;
        }

        [SerializeField] private float moveSpeed = 1.2f;
        [SerializeField] private GameObject attackGameObject;
        private Attack attack; // TESTING ONLY, DELETE LATER

        private CharacterController controller;

        private readonly float interactDistance = 0.5f;
        private Vector3 lastInteractDir;
        private Interactions.IInteractable selectedInteractable;
        private GameInput gameInput;

        private PlayerDataManager playerDataManager;

        private void Awake() {
            attack = attackGameObject.GetComponent<Attack>();
            controller = GetComponent<CharacterController>();
        }

        public void Instantiate() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject); // make persistent across scenes
            }
            else {
                Destroy(gameObject); // delete duplicates
            }
            
            playerDataManager = new PlayerDataManager();
        }

        protected override void Start() {
            base.Start();
            
            gameInput = GameInput.Instance;

            gameInput.OnInteractAction += GameInput_OnInteractAction;
            gameInput.OnAttackAction += GameInput_OnAttackAction;
            gameInput.OnInventoryAction += GameInput_OnInventoryAction;
            gameInput.OnOpenCompanionMenuAction += GameInput_OnOpenCompanionMenuAction;
        }
        
        private void GameInput_OnOpenCompanionMenuAction(object sender, EventArgs e) {
            UIDocument companionMenu = FindObjectOfType<UIDocument>();
            if (companionMenu != null)
            {
                companionMenu.rootVisualElement.style.display = DisplayStyle.Flex;
            }
        }

        protected void OnDestroy() {
            gameInput = GameInput.Instance;
            gameInput.OnInteractAction += GameInput_OnInteractAction;
            gameInput.OnAttackAction += GameInput_OnAttackAction;
            gameInput.OnInventoryAction += GameInput_OnInventoryAction;
        }

        private void Update() {
            HandleMovement();
            HandleInteractions();

            // quit application
            if (Input.GetKeyDown("escape")) {
                Quit();
            }

            if (Input.GetKeyDown(KeyCode.M)) {
                Debug.Log(GameManager.Instance.inventoryManager.ToReadableString());
            }

            if (Input.GetKeyDown(KeyCode.O)) {
                Debug.Log(GameManager.Instance.dayCycleManager.GetStandardTime());
                Debug.Log(GameManager.Instance.dayCycleManager.GetCurrentDayPhase());
                Debug.Log(GameManager.Instance.worldManager.GetWorldDataManager().GetDay().day.ToString());
            }
            if (Input.GetKeyDown(KeyCode.P)) {
                GameManager.Instance.dayCycleManager.Pause();
            }
            if (Input.GetKeyDown(KeyCode.L)) {
                GameManager.Instance.dayCycleManager.Unpause();
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
                GameManager.Instance.shopManager.HandleOrderGeneration();
            }

            if (Input.GetKeyDown(KeyCode.Alpha6)) {
                GameManager.Instance.shopManager.GetShopSelectionScore();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha7)) {
                GameManager.Instance.shopManager.IncreaseShopReputationLevel();
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
        
        public PlayerDataManager GetPlayerDataManager() => playerDataManager;

        private void GameInput_OnInteractAction(object sender, EventArgs e) {
            selectedInteractable?.Interact();
        }

        private void GameInput_OnAttackAction(object sender, EventArgs e) {
            HandleCombat();
        }

        private void GameInput_OnInventoryAction(object sender, EventArgs e) {
            OnOpenInventory?.Invoke(this, EventArgs.Empty);
        }

        private void HandleCombat() {
            base.OnAttack();
            gameInput.DisableAllInputs();
        }

        public override void OnAttack() {
            combatController.Attack(attack);
        }

        public override void OnAttackFinish() {
            gameInput.EnableInputMapOnly(ActionMap.Default);
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
            FlipRelevantPlayerAssets(inputVector);
        }

        private void SetSelectedInteractable(Interactions.IInteractable interactable) {
            this.selectedInteractable = interactable;

            OnSelectedInteractableChanged?.Invoke(this, new OnSelectedInteractableChangedEventArgs {
                selectedInteractable = selectedInteractable
            });
        }

        private void FlipRelevantPlayerAssets(Vector3 normalizedVector) {
            if (normalizedVector.x > 0) {
                attackGameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            } else if (normalizedVector.x < 0) {
                attackGameObject.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
        }
    }
}