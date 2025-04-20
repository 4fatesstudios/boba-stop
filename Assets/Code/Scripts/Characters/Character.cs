using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Characters
{
    public abstract class Character : MonoBehaviour {
        private SpriteRenderer spriteRenderer;
        protected bool isWalking;
        protected readonly float Gravity = 1.0f;

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
    }
}
