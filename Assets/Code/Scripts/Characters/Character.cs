using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BobaStop.Characters.Animation;

namespace BobaStop.Characters
{
    public enum Direction { Down, Left, Right, Up }
    public enum Action { Idle, Walk, Attack, Mining }
    
    public abstract class Character : MonoBehaviour {
        private SpriteRenderer spriteRenderer;
        protected bool isWalking;
        protected readonly float Gravity = 1.0f;

        private Direction currentFacingDirection;
        private Action currentAction;
        private bool isActionLocked = false;

        protected virtual void Start() {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        
        protected void FlipSpriteToForwardVector(Vector3 normalizedVector) {
            spriteRenderer.flipX = normalizedVector.x == 0 ? spriteRenderer.flipX : normalizedVector.x < 0;
        }

        public bool GetIsFacingRight() {
            return !spriteRenderer.flipX;
        }
        
        public bool IsWalking() {
            return isWalking;
        }

        public void UpdateFacedDirection(Direction direction) {
            currentFacingDirection = direction;
        }

        public void UpdateCurrentAction(Action action) {
            if (isActionLocked) return;
            
            currentAction = action;
        }

        protected void LockAction(Action action) {
            UpdateCurrentAction(action);
            isActionLocked = true;
        }

        protected void UnlockAction() {
            isActionLocked = false;
        }

        public Direction GetCurrentFacingDirection() {
            return currentFacingDirection;
        }

        public Action GetCurrentAction() {
            return currentAction;
        }
    }
}
