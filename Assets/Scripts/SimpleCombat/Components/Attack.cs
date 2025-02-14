using UnityEngine;

namespace SimpleCombat.Components {
    public class Attack : MonoBehaviour {
        [SerializeField] private int damage = 0;
        [SerializeField] private BoxCollider hurtbox = null;

        public int GetDamage() {
            return damage;
        }

        public BoxCollider GetHurtbox() {
            return hurtbox;
        }
    }
}