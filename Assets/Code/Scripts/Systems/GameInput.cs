using System;
using UnityEngine;

namespace BobaStop.Systems {
    public class GameInput : MonoBehaviour {

        public event EventHandler OnInteractAction;
        public event EventHandler OnAttackAction;
        public event EventHandler OnInventoryAction;

        private PlayerInputActions playerInputActions;

        private void Awake() {
            playerInputActions = new PlayerInputActions();
            playerInputActions.Player.Enable();

            playerInputActions.Player.Interact.performed += Interact_performed;
            playerInputActions.Player.Attack.performed += Attack_performed;
            playerInputActions.Player.Inventory.performed += InventoryAction_performed;
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