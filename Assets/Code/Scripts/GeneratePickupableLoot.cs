using System.Collections;
using System.Collections.Generic;
using BobaStop.Interactions;
using BobaStop.LootTables;
using UnityEngine;

namespace BobaStop
{
    public class GeneratePickupableLoot : MonoBehaviour
    {
        [SerializeField] private LootTable lootTable;
        [SerializeField] private Transform lootGenerationOrigin;
        [SerializeField] private GameObject pickupPrefab;
        private float initialVelocityStrength = 2f;

        public void Initialize(LootTable lootTable, Transform lootGenerationOrigin, GameObject pickupPrefab) {
            this.lootTable = lootTable;
            this.lootGenerationOrigin = lootGenerationOrigin;
            this.pickupPrefab = pickupPrefab;
        }
        
        public void GenerateLoot() {
            Debug.Log("Generating pickupable loot");
            foreach (var lootEntry in lootTable) {
                int generatedLootAmount = Random.Range(lootEntry.GetMinimumDropAmount(), lootEntry.GetMaximumDropAmount());
                
                for (int i = 0; i < generatedLootAmount; i++) {
                    GameObject lootInstance = Instantiate(pickupPrefab, lootGenerationOrigin.position, Quaternion.identity);
                    lootInstance.GetComponent<InteractablePickupable>().SetItem(lootEntry.GetItem());
                    
                    Rigidbody rb = lootInstance.GetComponent<Rigidbody>();
                    
                    Vector3 randomDirection = Random.onUnitSphere; // Random direction in all directions
                    randomDirection.y = Mathf.Abs(randomDirection.y);
                    
                    rb.velocity = randomDirection * Random.Range(0, initialVelocityStrength);
                }
            }
        }
    }
}
