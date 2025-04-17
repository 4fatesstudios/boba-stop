using System;
using BobaStop.Items;
using BobaStop.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace BobaStop.Inventory
{
    public class ItemSlotUI : VisualElement
    {
        
        private readonly Item m_Item;

        public ItemSlotUI (Item item)
        {
            m_Item = item;

            name = $"{m_Item.itemName}";
            style.height = m_Item.slotDimension.height * 
                           PlayerInventory.slotDimension.height;
            style.width = m_Item.slotDimension.width * 
                          PlayerInventory.slotDimension.width;
            style.visibility = Visibility.Hidden;

            VisualElement icon = new VisualElement
            {
                style = { backgroundImage = m_Item.itemSprite.texture }
            };
            Add(icon);

            icon.AddToClassList("visual-icon");
            AddToClassList("visual-icon-container");
        }

        public void SetPosition(Vector2 pos)
        {
            style.left = pos.x;
            style.top = pos.y;
        }
        
        [Serializable]
        public class StoredItem
        {
            public Item details;
            public ItemSlotUI RootVisual;
        }
        
        // [SerializeField] private RectTransform rectTransform;
        // [SerializeField] private float slotSize = 50.0f;
        
        // public bool allowResources = true; // Allow Resource items
        // public bool allowWeapons = true;   // Allow Weapon items
        // public bool allowPacks = true;    // Allow Pack items
        // public bool allowGear = true;     // Allow Gear items

        // [FormerlySerializedAs("currentItem")] public ItemUI currentItemUI; // The item currently in the slot

        // public void Awake() {
        //     if (rectTransform != null) {
        //         rectTransform.sizeDelta = new Vector2(slotSize, slotSize);
        //     }
        // }
        //
        // public void OnDrop(PointerEventData eventData)
        // {
        //     if (!InventoryToggle.isInventoryVisible) return;
        //     if (transform.childCount == 0) // Check if the slot is empty
        //     {
        //         GameObject dropped = eventData.pointerDrag;
        //         ItemUI itemUI = dropped.GetComponent<ItemUI>();
        //
        //         // Check if the item is allowed in this slot
        //         if (IsItemAllowed(itemUI.item))
        //         {
        //             // Set the current item in the slot
        //             currentItemUI = itemUI;
        //             itemUI.parentAfterDrag = transform;
        //         }
        //         else
        //         {
        //             Debug.Log("This item is not allowed in this slot.");
        //         }
        //     }
        // }
        //
        // // Helper method to check if the item is allowed
        // private bool IsItemAllowed(Item item)
        // {
        //     if (item is Resource && allowResources) return true;
        //     if (item is Weapon && allowWeapons) return true;
        //     if (item is Pack && allowPacks) return true;
        //     if (item is Gear && allowGear) return true;
        //     return false;
        // }
        //
        // // Remove the item from this slot
        // public void RemoveItem()
        // {
        //     currentItemUI = null;
        // }
    }
}