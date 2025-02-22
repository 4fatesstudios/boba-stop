using UnityEngine;

namespace SimpleCombat.Components {
    public class Hitbox : MonoBehaviour {
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private CombatController combatController;
        
        public void Start() {
            if (boxCollider == null) {
                Debug.LogWarning("SimpleCombat: No box collider on " + gameObject.name);
            }
            
        }

        public CombatController GetCombatController() {
            return combatController;
        }

        public BoxCollider GetBoxCollider() {
            return boxCollider;
        }
    }
}