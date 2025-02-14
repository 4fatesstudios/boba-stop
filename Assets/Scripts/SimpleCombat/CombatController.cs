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
                        combatController.TakeDamage(attackComponent.GetDamage());
                    }
                }
            }
        }

        private void TakeDamage(int damage) {
            healthComponent.SetCurrentHealth(healthComponent.GetCurrentHealth() - damage);
            if (healthComponent.GetCurrentHealth() <= 0) {
                // notify gameobject of death
                Debug.Log("dies");
            }
            else {
                // notify gameobject of damage taken
                Debug.Log("take damage");
            }
        }
    }
}