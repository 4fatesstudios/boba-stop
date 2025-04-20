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
        
        private VisualElement mRoot;
        private VisualElement mInventoryGrid;

        private static Label _mItemDetailHeader;
        private static Label _mItemDetailBody;
        private static Label _mItemDetailPrice;
        private bool mIsInventoryReady;
        public static ItemUI.Dimensions slotDimension { get; private set; }

        private async void Configure()
        {
            mRoot = GetComponentInChildren<UIDocument>().rootVisualElement;
            mInventoryGrid = mRoot.Q<VisualElement>("Grid");

            var itemDetails = mRoot.Q<VisualElement>("ItemDetails");

            _mItemDetailHeader = itemDetails.Q<Label>("Header");
            _mItemDetailBody = itemDetails.Q<Label>("Body");
            _mItemDetailPrice = itemDetails.Q<Label>("SellPrice");

            ConfigureInventoryTelegraph();

            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);

            ConfigureSlotDimensions();

            mIsInventoryReady = true;
        }

        private void ConfigureSlotDimensions()
        {
            var firstSlot = mInventoryGrid.Children().First();

            slotDimension = new ItemUI.Dimensions
            {
                width = Mathf.RoundToInt(firstSlot.worldBound.width),
                height = Mathf.RoundToInt(firstSlot.worldBound.height)
            };
        }

        public List<ItemSlotUI.StoredItem> storedItems = new();
        public ItemUI.Dimensions inventoryDimensions;

        private async Task<bool> GetPositionForItem(VisualElement newItem)
        {
            for (var y = 0; y < inventoryDimensions.height; y++)
            for (var x = 0; x < inventoryDimensions.width; x++)
            {
                // try position
                SetItemPosition(newItem, new Vector2(slotDimension.width * x,
                    slotDimension.height * y));

                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);

                var overlappingItem = storedItems.FirstOrDefault(s =>
                    s.RootVisual != null &&
                    s.RootVisual.layout.Overlaps(newItem.layout));

                // nothing here, place item
                if (overlappingItem == null) return true;
            }

            return false;
        }

        private static void SetItemPosition(VisualElement element, Vector2 vector)
        {
            element.style.left = vector.x;
            element.style.top = vector.y;
        }

        private void Start()
        {
            LoadInventory();
        }

        private async void LoadInventory()
        {
            await UniTask.WaitUntil(() => mIsInventoryReady);

            foreach (var loadedItem in storedItems)
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

        private void AddItemToInventoryGrid(VisualElement item) => mInventoryGrid.Add(item);
        private void RemoveItemFromInventoryGrid(VisualElement item) => mInventoryGrid.Remove(item);

        private static void ConfigureInventoryItem(ItemSlotUI.StoredItem item, ItemSlotUI visual)
        {
            item.RootVisual = visual;
            visual.style.visibility = Visibility.Visible;
        }
        
        private VisualElement mTelegraph;

        private void ConfigureInventoryTelegraph()
        {
            mTelegraph = new VisualElement
            {
                name = "Telegraph",
                style =
                {
                    position = Position.Absolute,
                    visibility = Visibility.Hidden
                }
            };

            mTelegraph.AddToClassList("slot-icon-highlighted");
            AddItemToInventoryGrid(mTelegraph);
        }
        
        public (bool canPlace, Vector2 position) ShowPlacementTarget(ItemSlotUI draggedItem)
        {
            if (!mInventoryGrid.layout.Contains(new Vector2(draggedItem.localBound.xMax,
                    draggedItem.localBound.yMax)))
            {
                mTelegraph.style.visibility = Visibility.Hidden;
                return (canPlace: false, position: Vector2.zero);
            }

            VisualElement targetSlot = mInventoryGrid.Children().Where(x => 
                x.layout.Overlaps(draggedItem.layout) && x != draggedItem).OrderBy(x => 
                Vector2.Distance(x.worldBound.position, 
                    draggedItem.worldBound.position)).First();

            mTelegraph.style.width = draggedItem.style.width;
            mTelegraph.style.height = draggedItem.style.height;

            SetItemPosition(mTelegraph, new Vector2(targetSlot.layout.position.x,
                targetSlot.layout.position.y));

            mTelegraph.style.visibility = Visibility.Visible;

            var overlappingItems = storedItems.Where(x => x.RootVisual != null && 
                                                          x.RootVisual.layout.Overlaps(mTelegraph.layout)).ToArray();

            if (overlappingItems.Length > 1)
            {
                mTelegraph.style.visibility = Visibility.Hidden;
                return (canPlace: false, position: Vector2.zero);
            }

            return (canPlace: true, targetSlot.worldBound.position);

        }
    }
}