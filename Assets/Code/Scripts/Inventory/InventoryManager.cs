using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.Items;
using UnityEngine;

namespace BobaStop.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public InventorySlot[] inventorySlots;
        public GameObject inventoryItemPrefab; // Prefab for the inventory item
        public List<Item> items;
        
        

        public bool AddItem(Item item)
        {
            // Find the first empty slot
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventorySlot slot = inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null)
                {
                    SpawnNewItem(item, slot);
                    items.Add(item);
                    return true;
                }
            }

            // No empty slots available
            return false;
        }

        private void SpawnNewItem(Item item, InventorySlot slot)
        {
            // Instantiate the new item prefab
            GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
            InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
            inventoryItem.InitializeItem(item);

            // Set the current item in the slot
            slot.currentItem = inventoryItem;
        }
    }
}