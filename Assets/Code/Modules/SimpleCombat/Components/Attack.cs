using UnityEngine;

namespace SimpleCombat.Components {
    public class Attack : MonoBehaviour {
        [SerializeField] private int damage;
        [SerializeField] private BoxCollider attackbox;
        [SerializeField] private CombatController combatController;

        public int GetDamage() {
            return damage;
        }

        public BoxCollider GetAttackbox() {
            return attackbox;
        }
        
        public CombatController GetCombatController() {
            return combatController;
        }
    }
}