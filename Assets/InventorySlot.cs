using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public ItemType allowedItemType;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>();

            if (inventoryItem != null && IsValidForSlot(inventoryItem))
            {
                inventoryItem.parentAfterDrag = transform;
            }
            else
            {
                Debug.LogWarning("This item cannot be placed in this slot.");
            }
        }
    }

    // check if the item is valid for the slot based on its type
    private bool IsValidForSlot(InventoryItem inventoryItem)
    {
        Item item = inventoryItem.GetComponent<Item>();
        if (item != null && item.itemType == allowedItemType)
        {
            return true;
        }
        return false;
    }
}