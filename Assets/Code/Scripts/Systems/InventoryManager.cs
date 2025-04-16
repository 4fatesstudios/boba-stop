using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.Data.Saved;
using BobaStop.Items;
using BobaStop.Systems;
using UnityEngine;

namespace BobaStop.Systems
{
    public class InventoryManager : IPersistenceData
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
            
            this.inventoryData.inventory = new ItemSlotContainer<Item>(inventoryData.inventory);
            this.inventoryData.gear = new ItemSlotContainer<Gear>(inventoryData.gear);
            this.inventoryData.weapon = new ItemSlotContainer<Weapon>(inventoryData.weapon);
            this.inventoryData.inventoryLevel = inventoryData.inventoryLevel;
        }
        
        public void SaveData(SaveData saveData) {
            if (saveData is not InventoryData inventoryData) {
                Debug.LogWarning("Save data is not a InventoryData");
                return;
            }
            
            inventoryData.inventory = new ItemSlotContainer<Item>(this.inventoryData.inventory);
            inventoryData.gear = new ItemSlotContainer<Gear>(this.inventoryData.gear);
            inventoryData.weapon = new ItemSlotContainer<Weapon>(this.inventoryData.weapon);
            inventoryData.inventoryLevel = this.inventoryData.inventoryLevel;
        }

        public void UpdateInventorySpace() {
            inventoryLevelSlots.TryGetValue(inventoryData.inventoryLevel, out int slots);
            inventoryData.inventory.AddSlots(slots - inventoryData.inventory.GetSlots());
        }

        public void IncreaseInventoryLevel() {
            ++inventoryData.inventoryLevel;
        }

        public string ToReadableString() {
            return inventoryData.inventory.ToReadableString();
        }

        /// <summary>
        /// Adds item and quantity to inventory
        /// </summary>
        /// <param name="item">item to add to inventory</param>
        /// <param name="quantity">quantity of item to add to inventory</param>
        /// <returns>remainder of items that couldn't be added, 0 if all added</returns>
        public int AddItemToInventory(Item item, int quantity) {
            return inventoryData.inventory.AddItem(item, quantity);
        }
    }
}