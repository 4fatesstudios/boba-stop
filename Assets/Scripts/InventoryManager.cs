using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot weaponSlot;
    public InventorySlot armorSlot;

    public void AddItem(Item item)
    {
        
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        
    }

    // public void AddItemToSlot(GameObject item, InventorySlot slot)
    // {
    //     InventoryItem inventoryItem = item.GetComponent<InventoryItem>();
    //
    //     if (inventoryItem != null && slot.IsValidForSlot(inventoryItem))
    //     {
    //         inventoryItem.parentAfterDrag = slot.transform;
    //         slot.AddItem(item);
    //         Debug.Log($"{inventoryItem.GetComponent<Item>().itemName} added to the {slot.allowedItemType} slot!");
    //     }
    //     else
    //     {
    //         Debug.LogWarning("This item cannot be placed in this slot.");
    //     }
    // }
}