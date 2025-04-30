using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace BobaStop.Data
{
    [CreateAssetMenu(menuName = "Data/Shop Reputation", fileName = "ShopReputationData")]
    public class ShopReputationData : ScriptableObject {
        [SerializeField] private ShopReputation[] reputationLevels;

        private Dictionary<int, ShopReputation> reputationLookup;

        private void OnEnable() {
            reputationLookup = new Dictionary<int, ShopReputation>();
            foreach (var reputation in reputationLevels) {
                reputationLookup[reputation.level] = reputation;
            }
        }

        public ShopReputation GetReputation(int level) {
            if (reputationLookup.TryGetValue(level, out var reputation)) {
                return reputation;
            }
            Debug.LogWarning($"No reputation found for level {level}");
            return default;
        }

    }
    
    [System.Serializable]
    public struct ShopReputation {
        public int level;
        public int expToLevel;
        public float generatedOrdersMultiplier;
        public int shopInventorySize;
        public int shopSelectionSize;

        public ShopReputation(int level, int expToLevel, int generatedOrdersMultiplier, int shopInventorySize, int shopSelectionSize) {
            this.level = level;
            this.expToLevel = expToLevel;
            this.generatedOrdersMultiplier = generatedOrdersMultiplier;
            this.shopInventorySize = shopInventorySize;
            this.shopSelectionSize = shopSelectionSize;
        }
    }
}
