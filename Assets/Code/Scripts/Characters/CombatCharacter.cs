using System;
using SimpleCombat;
using SimpleCombat.Components;

namespace BobaStop.Characters {
    public abstract class CombatCharacter : Character, IDamageable {
        public event EventHandler OnAttackAction;

        private SpriteMaterial _spriteMaterial;
        protected CombatController combatController;

        protected override void Start() {
            base.Start();
            _spriteMaterial = GetComponent<SpriteMaterial>();
            combatController = GetComponent<CombatController>();
        }

        public virtual void OnDamageTaken(Attack attackComponent, CombatController source) {
            _spriteMaterial.CallDamageFlash();
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