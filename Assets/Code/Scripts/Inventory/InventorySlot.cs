using BobaStop.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BobaStop.Inventory
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public bool allowResources = true; // Allow Resource items
        public bool allowWeapons = true;   // Allow Weapon items
        public bool allowPacks = true;    // Allow Pack items
        public bool allowGear = true;     // Allow Gear items

        public InventoryItem currentItem; // The item currently in the slot

        public void OnDrop(PointerEventData eventData)
        {
            if (!InventoryToggle.isInventoryVisible) return;
            if (transform.childCount == 0) // Check if the slot is empty
            {
                GameObject dropped = eventData.pointerDrag;
                InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>();

                // Check if the item is allowed in this slot
                if (IsItemAllowed(inventoryItem.item))
                {
                    // Set the current item in the slot
                    currentItem = inventoryItem;
                    inventoryItem.parentAfterDrag = transform;
                }
                else
                {
                    Debug.Log("This item is not allowed in this slot.");
                }
            }
        }

        // Helper method to check if the item is allowed
        private bool IsItemAllowed(Item item)
        {
            if (item is Resource && allowResources) return true;
            if (item is Weapon && allowWeapons) return true;
            if (item is Pack && allowPacks) return true;
            if (item is Gear && allowGear) return true;
            return false;
        }

        // Remove the item from this slot
        public void RemoveItem()
        {
            currentItem = null;
        }
    }
}