using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace BobaStop.LootTables
{
    [System.Serializable]
    public class LootTableEntry {
        [SerializeField] private int minimumDropAmount;
        [SerializeField] private int maximumDropAmount;
        [FormerlySerializedAs("loot")] [SerializeField] private Items.Item item;
        private const int MAX = 100;

        public void Validate(LootTable parentLootTable) {
            minimumDropAmount = Mathf.Clamp(minimumDropAmount, 0, maximumDropAmount);
            maximumDropAmount = Mathf.Clamp(maximumDropAmount, minimumDropAmount, MAX);
            if (item == null) {
                Debug.LogWarning($"Missing loot in '{parentLootTable.name}'!", parentLootTable);
            }
        }

        public int GetMinimumDropAmount() => minimumDropAmount;
        public int GetMaximumDropAmount() => maximumDropAmount;
        public Items.Item GetItem() => item;
    }
    
    [CreateAssetMenu(menuName = "Loot Table", fileName = "LootTable")]
    public class LootTable : ScriptableObject, IEnumerable<LootTableEntry>  {
        [SerializeField] private LootTableEntry[] lootEntries;

        private void OnValidate() {
            foreach (LootTableEntry lootEntry in lootEntries) {
                lootEntry?.Validate(this);
            }
        }
        
        // enables foreach iteration
        public IEnumerator<LootTableEntry> GetEnumerator() {
            foreach (var entry in lootEntries) {
                yield return entry;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
