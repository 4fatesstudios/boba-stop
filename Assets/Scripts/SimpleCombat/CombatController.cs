using System;
using UnityEngine;

namespace SimpleCombat {
    internal enum Faction {
        Unset,
        Friendly,
        Enemy
    }

    public class CombatController : MonoBehaviour {
        
        [SerializeField] private Faction faction = Faction.Unset;
        [SerializeField] private Components.Health healthComponent = null;
        [SerializeField] private Components.Hitbox hitboxComponent = null;
        
        private IDamageable damageable;

        private void Awake() {
            if (!TryGetComponent(out damageable)) {
                Debug.LogWarning("No interface of type " + typeof(IDamageable) + " in " + gameObject.name);
            }
        }
        private void Start() {
            if (faction == Faction.Unset) {
                Debug.LogWarning("SimpleCombat: Faction is unset on " + gameObject.name);
            }
            if (healthComponent == null || hitboxComponent == null) {
                Debug.LogWarning("SimpleCombat: No health/hitbox component found on " + gameObject.name);
            }
        }
        
        public void Attack(Components.Attack attackComponent) {
            var colliders = Physics.OverlapBox(
                attackComponent.GetHurtbox().bounds.center,
                attackComponent.GetHurtbox().bounds.extents,
                attackComponent.GetHurtbox().transform.rotation);
            
            foreach (var collider in colliders) {
                if (collider is BoxCollider &&
                    collider.gameObject.TryGetComponent(out CombatController combatController)) {
                    if (combatController.faction != faction) {
                        combatController.TakeDamage(attackComponent, this);
                    }
                }
            }
        }
        
        private void TakeDamage(Components.Attack attackComponent, CombatController source) {
            healthComponent.SetCurrentHealth(Mathf.Max(0, healthComponent.GetCurrentHealth() - attackComponent.GetDamage()));
            if (healthComponent.GetCurrentHealth() <= 0) {
                Debug.Log("lethal damage taken");
                damageable?.OnDeath(attackComponent, source);
            }
            else {
                Debug.Log("non-lethal damage taken");
                damageable?.OnDamageTaken(attackComponent, source);
            }
        }
    }
}