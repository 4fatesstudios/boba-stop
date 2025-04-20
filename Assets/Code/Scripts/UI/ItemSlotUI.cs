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


            RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
            RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
            RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
        }
        
        [Serializable]
        public class StoredItem
        {
            public Item details;
            public ItemSlotUI RootVisual;
        }
        
        // click and drag logic
        private Vector2 mOriginalPosition;
        private bool mIsDragging;
        private (bool canPlace, Vector2 position) mPlacementResults;

        private void OnMouseDownEvent(MouseDownEvent mouseEvent)
        {
            Debug.Log("Mouse down");
            StartDrag();
        }
        
        private void OnMouseMoveEvent(MouseMoveEvent mouseEvent)
        {
            if (!mIsDragging) return; 

            SetPosition(GetMousePosition(mouseEvent.mousePosition));
            mPlacementResults = PlayerInventory.instance.ShowPlacementTarget(this);
        }
        
        private void OnMouseUpEvent(MouseUpEvent mouseEvent)
        {
            Debug.Log("Mouse up");
            if (!mIsDragging) return;

            mIsDragging = false;
            
            if (mPlacementResults.canPlace)
            {
                Debug.Log("Item placed");
                SetPosition(new Vector2(
                    mPlacementResults.position.x - parent.worldBound.position.x,
                    mPlacementResults.position.y - parent.worldBound.position.y));
                return;
            }

            Debug.Log("Item not placed");
            SetPosition(new Vector2(mOriginalPosition.x, mOriginalPosition.y));
        }

        private void StartDrag()
        {
            mIsDragging = true;
            mOriginalPosition = worldBound.position - parent.worldBound.position;
            BringToFront();
        }
        
        private void SetPosition(Vector2 pos)
        {
            style.left = pos.x;
            style.top = pos.y;
        }

        private Vector2 GetMousePosition(Vector2 mousePosition) => 
            new Vector2(mousePosition.x - (layout.width / 2) - 
                        parent.worldBound.position.x, mousePosition.y - (layout.height / 2) - 
                                                      parent.worldBound.position.y);
        
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