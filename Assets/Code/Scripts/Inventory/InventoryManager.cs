using System.Collections;
using System.Collections.Generic;
using BobaStop.Inventory;
using BobaStop.Items;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                // SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    // void SpawnNewItem(Item item, InventorySlot slot)
    // {
    //     
    // }
}