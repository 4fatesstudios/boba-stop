using System;
using System.Collections;
using System.Collections.Generic;
using SimpleCombat;
using SimpleCombat.Components;
using UnityEngine;

namespace BobaStop.Characters {
    public abstract class CombatCharacter : Character, IDamageable {
        public event EventHandler OnAttackAction;

        private DamageFlash damageFlash;
        protected CombatController combatController;

        protected override void Start() {
            base.Start();
            damageFlash = GetComponent<DamageFlash>();
            combatController = GetComponent<CombatController>();
        }

        public virtual void OnDamageTaken(Attack attackComponent, CombatController source) {
            damageFlash.CallDamageFlash();
        }

        public virtual void OnDeath(Attack attackComponent, CombatController source) {
            Destroy(gameObject);
        }

        public virtual void OnAttack() {
            OnAttackAction?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnAttackFinish() { }
    }
}