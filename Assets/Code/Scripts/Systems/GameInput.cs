using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BobaStop.Systems {

    public enum ActionMap {
        Default,
        Dialogue
    }
    
    public class GameInput : MonoBehaviour {
        public static GameInput Instance { get; private set; }

        public event EventHandler OnInteractAction;
        public event EventHandler OnAttackAction;
        public event EventHandler OnInventoryAction;

        public event EventHandler OnSkipAction;
        public event EventHandler OnEnterInputAction;
        public event EventHandler OnEndDialogueAction;

        private PlayerInputActions playerInputActions;
        private Dictionary<ActionMap, InputActionMap> mapDict;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject); // make persistent across scenes
            }
            else {
                Destroy(gameObject); // delete duplicates
            }
            
            playerInputActions = new PlayerInputActions();
            
            mapDict = new() {
                {ActionMap.Default, playerInputActions.Player},
                {ActionMap.Dialogue, playerInputActions.PlayerDialogue}
            };
            
            
            playerInputActions.Player.Enable();

            playerInputActions.Player.Interact.performed += Interact_performed;
            playerInputActions.Player.Attack.performed += Attack_performed;
            playerInputActions.Player.Inventory.performed += InventoryAction_performed;

            playerInputActions.PlayerDialogue.Skip.performed += Skip_performed;
            playerInputActions.PlayerDialogue.EnterInput.performed += EnterInput_performed;
            playerInputActions.PlayerDialogue.EndDialogue.performed += EndDialogue_performed;
        }

        private void OnDestroy() {
            playerInputActions.Player.Interact.performed -= Interact_performed;
            playerInputActions.Player.Attack.performed -= Attack_performed;
            playerInputActions.Player.Inventory.performed -= InventoryAction_performed;

            playerInputActions.PlayerDialogue.Skip.performed -= Skip_performed;
            playerInputActions.PlayerDialogue.EnterInput.performed -= EnterInput_performed;
            playerInputActions.PlayerDialogue.EndDialogue.performed -= EndDialogue_performed;
        }

        public void EnableInputMapOnly(ActionMap actionMap) {
            playerInputActions.Disable();
            mapDict.GetValueOrDefault(actionMap).Enable();
        }

        private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
            OnInteractAction?.Invoke(this, EventArgs.Empty);
        }

        private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
            OnAttackAction?.Invoke(this, EventArgs.Empty);
        }

        private void InventoryAction_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
            OnInventoryAction?.Invoke(this, EventArgs.Empty);
        }

        public Vector2 GetMovementVectorNormalized() {
            Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

            inputVector = inputVector.normalized;

            return inputVector;
        }

        private void Skip_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
            OnSkipAction?.Invoke(this, EventArgs.Empty);
        }

        private void EnterInput_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
            OnEnterInputAction?.Invoke(this, EventArgs.Empty);
        }

        private void EndDialogue_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
            OnEndDialogueAction?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Enables all input actions.
        /// </summary>
        public void EnableAllInputs() {
            playerInputActions.Enable();
        }

        /// <summary>
        /// Disables all input actions.
        /// </summary>
        public void DisableAllInputs() {
            playerInputActions.Disable();
        }
    }
}