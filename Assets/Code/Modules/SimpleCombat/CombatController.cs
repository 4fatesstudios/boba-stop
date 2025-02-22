using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace SimpleCombat {
    internal enum Faction {
        Unset,
        Friendly,
        Enemy
    }

    public class CombatController : MonoBehaviour {
        
        [SerializeField] private Faction faction = Faction.Unset;
        private Components.Health healthComponent;
        [SerializeField] private GameObject hitboxComponent;
        private Components.Hitbox hitbox;
        
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

            if (hitboxComponent == null) {
                Debug.LogWarning("SimpleCombat: No hitboxComponent on " + gameObject.name);
            }
            else {
                hitbox = hitboxComponent.GetComponent<Components.Hitbox>();
            }

            healthComponent = GetComponent<Components.Health>();
            if (healthComponent == null || hitbox == null) {
                Debug.LogWarning("SimpleCombat: No health/hitbox component found on " + gameObject.name);
            }
        }

        public Components.Hitbox GetHitbox() {
            return hitbox;
        }
        
        public void Attack(Components.Attack attackComponent) {
            // Ensure the attackComponent and its attackbox are valid
            if (attackComponent == null || attackComponent.GetAttackbox() == null) {
                Debug.LogWarning("AttackComponent or its attackbox is null.");
                return;
            }

            // Get all colliders within the attack's hurtbox
            var colliders = Physics.OverlapBox(
                attackComponent.GetAttackbox().bounds.center,
                attackComponent.GetAttackbox().bounds.extents,
                attackComponent.GetAttackbox().transform.rotation);

            // Iterate through all colliders
            foreach (var collider in colliders) {
                // Check if the collider is a BoxCollider
                if (collider is BoxCollider) {
                    // Try to get the Hitbox component from the collider's GameObject
                    var hitbox = collider.GetComponent<Components.Hitbox>();

                    // Ensure the Hitbox component and its CombatController are valid
                    if (hitbox != null) {
                        // Check if the combatController's faction is different from this object's faction
                        if (hitbox.GetCombatController().faction != faction) {
                            // Apply damage to the target
                            hitbox.GetCombatController().TakeDamage(attackComponent, this);
                            Debug.Log(hitbox.GetCombatController().gameObject.name + " took damage");
                        }
                    }
                }
            }
        }
        
        private void TakeDamage(Components.Attack attackComponent, CombatController source) {
            healthComponent.SetCurrentHealth(Mathf.Max(0, healthComponent.GetCurrentHealth() - attackComponent.GetDamage()));
            if (healthComponent.GetCurrentHealth() <= 0) {
                // Debug.Log("lethal damage taken");
                damageable?.OnDeath(attackComponent, source);
            }
            else {
                // Debug.Log("non-lethal damage taken");
                damageable?.OnDamageTaken(attackComponent, source);
            }
        }
    }
}