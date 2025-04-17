using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace BobaStop.Inventory
{
    public sealed class PlayerInventory : MonoBehaviour
    {
        public static PlayerInventory instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                Configure();
            }
            else if (instance != this)
            {
                Destroy(this);
            }
        }
        
        private VisualElement m_Root;
        private VisualElement m_InventoryGrid;

        private static Label m_ItemDetailHeader;
        private static Label m_ItemDetailBody;
        private static Label m_ItemDetailPrice;
        private bool m_IsInventoryReady;
        public static ItemUI.Dimensions slotDimension { get; private set; }

        private async void Configure()
        {
            m_Root = GetComponentInChildren<UIDocument>().rootVisualElement;
            m_InventoryGrid = m_Root.Q<VisualElement>("Grid");

            VisualElement itemDetails = m_Root.Q<VisualElement>("ItemDetails");

            m_ItemDetailHeader = itemDetails.Q<Label>("Header");
            m_ItemDetailBody = itemDetails.Q<Label>("Body");
            m_ItemDetailPrice = itemDetails.Q<Label>("SellPrice");

            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);

            ConfigureSlotDimensions();

            m_IsInventoryReady = true;
        }

        private void ConfigureSlotDimensions()
        {
            VisualElement firstSlot = m_InventoryGrid.Children().First();

            slotDimension = new ItemUI.Dimensions
            {
                width = Mathf.RoundToInt(firstSlot.worldBound.width),
                height = Mathf.RoundToInt(firstSlot.worldBound.height)
            };
        }
        
        public List<ItemSlotUI.StoredItem> storedItems = new List<ItemSlotUI.StoredItem>();
        public ItemUI.Dimensions inventoryDimensions;

        private async Task<bool> GetPositionForItem(VisualElement newItem)
        {
            for (int y = 0; y < inventoryDimensions.height; y++)
            {
                for (int x = 0; x < inventoryDimensions.width; x++)
                {
                    // try position
                    SetItemPosition(newItem, new Vector2(slotDimension.width * x, 
                        slotDimension.height * y));

                    await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);

                    ItemSlotUI.StoredItem overlappingItem = storedItems.FirstOrDefault(s => 
                        s.RootVisual != null && 
                        s.RootVisual.layout.Overlaps(newItem.layout));

                    // nothing here, place item
                    if (overlappingItem == null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static void SetItemPosition(VisualElement element, Vector2 vector)
        {
            element.style.left = vector.x;
            element.style.top = vector.y;
        }
        
        private void Start() => LoadInventory();

        private async void LoadInventory()
        {
            await UniTask.WaitUntil(() => m_IsInventoryReady);

            foreach (ItemSlotUI.StoredItem loadedItem in storedItems)
            {
                ItemSlotUI inventoryItemVisual = new ItemSlotUI(loadedItem.details);
                
                AddItemToInventoryGrid(inventoryItemVisual);

                bool inventoryHasSpace = await GetPositionForItem(inventoryItemVisual);

                if (!inventoryHasSpace)
                {
                    Debug.Log("No space - Cannot pick up the item");
                    RemoveItemFromInventoryGrid(inventoryItemVisual);
                    continue;
                }

                ConfigureInventoryItem(loadedItem, inventoryItemVisual);
            }
        }

        private void AddItemToInventoryGrid(VisualElement item) => m_InventoryGrid.Add(item);
        private void RemoveItemFromInventoryGrid(VisualElement item) => m_InventoryGrid.Remove(item);

        private static void ConfigureInventoryItem(ItemSlotUI.StoredItem item, ItemSlotUI visual)
        {
            item.RootVisual = visual;
            visual.style.visibility = Visibility.Visible;
        }
    }
}