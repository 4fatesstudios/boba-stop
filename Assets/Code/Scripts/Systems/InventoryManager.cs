using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.Items;
using BobaStop.UI;
using UnityEngine;

namespace BobaStop.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public int maxStackedItems = 10;
        public ItemSlotUI[] inventorySlots;
        public GameObject inventoryItemPrefab; // Prefab for the inventory item
        public List<Item> items;

        public ItemSlotUI[] toolbar1Slots;
        public ItemSlotUI[] toolbar2Slots;

        public bool AddItem(Item item)
        {
            // check if any slot has the same item with count lower than max
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                ItemSlotUI slotUI = inventorySlots[i];
                ItemUI itemUIInSlot = slotUI.GetComponentInChildren<ItemUI>();
                if (itemUIInSlot != null && itemUIInSlot.item == item && itemUIInSlot.count < maxStackedItems && itemUIInSlot.item.itemStackable)
                {
                    itemUIInSlot.count++;
                    itemUIInSlot.RefreshCount();
                    SyncToolbars();
                    return true;
                }
            }
            
            // Find the first empty slot
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                ItemSlotUI slotUI = inventorySlots[i];
                ItemUI itemUIInSlot = slotUI.GetComponentInChildren<ItemUI>();
                if (itemUIInSlot == null)
                {
                    SpawnNewItem(item, slotUI);
                    items.Add(item);
                    SyncToolbars();
                    return true;
                }
            }

            // No empty slots available
            return false;
        }

        private void SpawnNewItem(Item item, ItemSlotUI slotUI)
        {
            // Instantiate the new item prefab
            GameObject newItemGo = Instantiate(inventoryItemPrefab, slotUI.transform);
            ItemUI itemUI = newItemGo.GetComponent<ItemUI>();
            itemUI.InitializeItem(item);

            // Set the current item in the slot
            slotUI.currentItemUI = itemUI;
        }
        
        private void SyncToolbars()
        {
            for (int i = 0; i < toolbar2Slots.Length; i++)
            {
                if (i < toolbar1Slots.Length)
                {
                    ItemSlotUI slot1 = toolbar1Slots[i];
                    ItemSlotUI slot2 = toolbar2Slots[i];

                    if (slot1.currentItemUI != null)
                    {
                        if (slot2.currentItemUI == null)
                        {
                            SpawnNewItem(slot1.currentItemUI.item, slot2);
                        }
                        else
                        {
                            slot2.currentItemUI.item = slot1.currentItemUI.item;
                            slot2.currentItemUI.count = slot1.currentItemUI.count;
                            slot2.currentItemUI.RefreshCount();
                        }
                    }
                    else
                    {
                        slot2.RemoveItem();
                    }
                }
            }
        }
        
    }
}