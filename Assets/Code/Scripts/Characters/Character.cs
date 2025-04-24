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

        protected void UpdateFacedDirection(Direction direction) {
            currentFacingDirection = direction;
        }

        protected void UpdateCurrentAction(Action action) {
            currentAction = action;
        }

        public Direction GetCurrentFacingDirection() {
            return currentFacingDirection;
        }

        public Action GetCurrentAction() {
            return currentAction;
        }
    }
}
