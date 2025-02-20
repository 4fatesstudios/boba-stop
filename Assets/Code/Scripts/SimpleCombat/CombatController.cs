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
        private Components.Health healthComponent;
        [SerializeField] private GameObject gameObjectHitbox;
        private Components.Hitbox hitboxComponent;
        
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
            healthComponent = GetComponent<Components.Health>();
            hitboxComponent = gameObjectHitbox.GetComponent<Components.Hitbox>();
            if (healthComponent == null || hitboxComponent == null) {
                Debug.LogWarning("SimpleCombat: No health/hitbox component found on " + gameObject.name);
            }
        }
        
        // public void Attack(Components.Attack attackComponent) {
        //     var colliders = Physics.OverlapBox(
        //         attackComponent.GetHurtbox().bounds.center,
        //         attackComponent.GetHurtbox().bounds.extents,
        //         attackComponent.GetHurtbox().transform.rotation);
            
        //     foreach (var collider in colliders) {
        //         if (collider is BoxCollider &&
        //             collider.gameObject.gameObject.TryGetComponent(out CombatController combatController)) {
        //             if (combatController.faction != faction) {
        //                 combatController.TakeDamage(attackComponent, this);
        //             }
        //         }
        //     }
        // }
        public void Attack(Components.Attack attackComponent) {
            // Get all colliders within the attack's hurtbox
            var colliders = Physics.OverlapBox(
                attackComponent.GetHurtbox().bounds.center,
                attackComponent.GetHurtbox().bounds.extents,
                attackComponent.GetHurtbox().transform.rotation);

            // Iterate through all colliders
            foreach (var collider in colliders) {
                // Check if the collider is a BoxCollider and if its GameObject is named "Hitbox"
                if (collider is BoxCollider && collider.gameObject.name == "Hitbox") {
                    // Get the parent GameObject of the "Hitbox"
                    GameObject parentObject = collider.transform.parent.gameObject;

                    // Try to get the CombatController component from the parent GameObject
                    if (parentObject.TryGetComponent(out CombatController combatController)) {
                        // Check if the combatController's faction is different from this object's faction
                        if (combatController.faction != faction) {
                            // Apply damage to the target
                            combatController.TakeDamage(attackComponent, this);
                        }
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