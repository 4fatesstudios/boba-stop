using UnityEngine;

namespace SimpleCombat.Components {
    public class Health : MonoBehaviour {
        [SerializeField] private int maxHealth = 100;

        private int currentHealth;
        private int healthRegenRate;

        private void Start() {
            currentHealth = maxHealth;
        }

        private void Update() {
            currentHealth += healthRegenRate;
        }

        public int GetCurrentHealth() {
            return currentHealth;
        }

        public int GetMaxHealth() {
            return maxHealth;
        }

        public void SetCurrentHealth(int newHealth) {
            currentHealth = newHealth;
        }
    }
}