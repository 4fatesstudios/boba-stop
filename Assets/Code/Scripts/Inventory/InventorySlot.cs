using UnityEngine;
using UnityEngine.EventSystems;

namespace BobaStop.Inventory {
    public class InventorySlot : MonoBehaviour, IDropHandler {
        public ItemType allowedItemType;

        public void OnDrop(PointerEventData eventData) {
            if (transform.childCount == 0) {
                GameObject dropped = eventData.pointerDrag;
                InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>();
                inventoryItem.parentAfterDrag = transform;
            }
        }
    }
}