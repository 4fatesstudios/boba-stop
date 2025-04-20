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
    }
}