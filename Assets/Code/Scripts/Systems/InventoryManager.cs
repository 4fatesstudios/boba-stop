using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.Data.Saved;
using BobaStop.Items;
using BobaStop.Systems;
using UnityEngine;

namespace BobaStop.Inventory
{
    public class InventoryManager : MonoBehaviour, IPersistenceData
    {
        private InventoryData inventoryData = new();
        
        private Dictionary<int, int> inventoryLevelSlots = new() {
            {1, 12},
            {2, 18},
            {3, 24},
        };
        
        public void LoadData(SaveData saveData) {
            if (saveData is not InventoryData inventoryData) {
                Debug.LogWarning("Save data is not a InventoryData");
                return;
            }
            
            this.inventoryData.inventory = new(inventoryData.inventory);
            this.inventoryData.gear = new(inventoryData.gear);
            this.inventoryData.weapon = new(inventoryData.weapon);
            this.inventoryData.inventoryLevel = inventoryData.inventoryLevel;
        }
        
        public void SaveData(SaveData saveData) {
            if (saveData is not InventoryData inventoryData) {
                Debug.LogWarning("Save data is not a InventoryData");
                return;
            }
            
            inventoryData.inventory = new(this.inventoryData.inventory);
            inventoryData.gear = new(this.inventoryData.gear);
            inventoryData.weapon = new(this.inventoryData.weapon);
            inventoryData.inventoryLevel = this.inventoryData.inventoryLevel;
        }

        public void UpdateInventorySpace() {
            inventoryLevelSlots.TryGetValue(inventoryData.inventoryLevel, out int slots);
            inventoryData.inventory.AddSlots(slots - inventoryData.inventory.GetSlots());
        }

        public void AddItemToInventory(Item item) {
            // inventoryData.inventory.AddItem(item);
        }
    }
}