using System;
using System.Collections;
using System.Collections.Generic;
using SimpleCombat;
using SimpleCombat.Components;
using UnityEngine;

namespace BobaStop.Characters {
    public abstract class Character : MonoBehaviour, IDamageable {
        public event EventHandler OnAttackAction;

        private DamageFlash damageFlash;
        private SpriteRenderer spriteRenderer;
        protected CombatController combatController;
        protected bool isWalking;
        protected readonly float Gravity = 1.0f;

        protected virtual void Start() {
            damageFlash = GetComponent<DamageFlash>();
            combatController = GetComponent<CombatController>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public virtual void OnDamageTaken(Attack attackComponent, CombatController source) {
            damageFlash.CallDamageFlash();
        }

        public virtual void OnDeath(Attack attackComponent, CombatController source) {
            Destroy(gameObject);
        }

        protected void FlipSpriteToForwardVector(Vector3 normalizedVector) {
            spriteRenderer.flipX = normalizedVector.x == 0 ? spriteRenderer.flipX : normalizedVector.x < 0;
        }

        public bool IsWalking() {
            return isWalking;
        }

        protected virtual void OnAttack() {
            OnAttackAction?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnAttackAnimationFinish() { }
    }
}