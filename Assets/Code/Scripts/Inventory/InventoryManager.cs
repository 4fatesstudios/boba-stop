using UnityEngine;

namespace BobaStop.Inventory {
    public class InventoryManager : MonoBehaviour {
        public InventorySlot weaponSlot;
        public InventorySlot armorSlot;

        public InventorySlot[] inventorySlots;
        public GameObject inventoryItemPrefab;
    }
}