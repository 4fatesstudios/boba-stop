using System.Collections.Generic;
using BobaStop.Items;
using BobaStop.UI;
using UnityEngine;

namespace BobaStop.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public int maxStackedItems = 10;
        public List<ItemSlotUI> inventorySlots = new List<ItemSlotUI>();
        public GameObject inventoryItemPrefab;
        public GameObject inventorySlotPrefab; 
        public List<Item> items;

        public ItemSlotUI[] toolbar1Slots;
        public ItemSlotUI[] toolbar2Slots;
        public Transform mainInventoryTransform;

        private Dictionary<ItemSlotUI, Item> itemSlotDictionary;

        private void Awake()
        {
            itemSlotDictionary = new Dictionary<ItemSlotUI, Item>();
            InitializeSlots(6, inventorySlotPrefab, mainInventoryTransform, 50.0f);
        }

        public void InitializeSlots(int slotCount, GameObject itemSlotPrefab, Transform itemSlotContainer, float slotSize)
        {
            for (int i = 0; i < slotCount; i++)
            {
                AddSlot(itemSlotPrefab, itemSlotContainer, slotSize);
            }
        }

        public void AddSlot(GameObject itemSlotPrefab, Transform itemSlotContainer, float slotSize)
        {
            GameObject slotObject = Instantiate(itemSlotPrefab, itemSlotContainer);
            ItemSlotUI itemSlotUI = slotObject.GetComponent<ItemSlotUI>();
            itemSlotUI.Initialize(slotSize);
            inventorySlots.Add(itemSlotUI);
            itemSlotDictionary.Add(itemSlotUI, null);
        }

        public bool IsSlotValid(ItemSlotUI itemSlotUI)
        {
            return itemSlotDictionary.ContainsKey(itemSlotUI);
        }

        public bool IsSlotEmpty(ItemSlotUI itemSlotUI)
        {
            return itemSlotDictionary[itemSlotUI] == null;
        }

        public bool AddItem(Item item)
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                ItemSlotUI slotUI = inventorySlots[i];
                ItemUI itemUIInSlot = slotUI.GetComponentInChildren<ItemUI>();
                if (itemUIInSlot != null && itemUIInSlot.item == item && itemUIInSlot.count < maxStackedItems && itemUIInSlot.item.itemStackable)
                {
                    itemUIInSlot.count++;
                    itemUIInSlot.RefreshCount();
                    SyncToolbars();
                    return true;
                }
            }

            for (int i = 0; i < inventorySlots.Count; i++)
            {
                ItemSlotUI slotUI = inventorySlots[i];
                ItemUI itemUIInSlot = slotUI.GetComponentInChildren<ItemUI>();
                if (itemUIInSlot == null)
                {
                    SpawnNewItem(item, slotUI);
                    items.Add(item);
                    SyncToolbars();
                    return true;
                }
            }

            return false;
        }

        private void SpawnNewItem(Item item, ItemSlotUI slotUI)
        {
            GameObject newItemGo = Instantiate(inventoryItemPrefab, slotUI.transform);
            ItemUI itemUI = newItemGo.GetComponent<ItemUI>();
            itemUI.InitializeItem(item);

            slotUI.currentItemUI = itemUI;
            itemSlotDictionary[slotUI] = item;
        }

        private void SyncToolbars()
        {
            for (int i = 0; i < toolbar2Slots.Length; i++)
            {
                if (i < toolbar1Slots.Length)
                {
                    ItemSlotUI slot1 = toolbar1Slots[i];
                    ItemSlotUI slot2 = toolbar2Slots[i];

                    if (slot1.currentItemUI != null)
                    {
                        if (slot2.currentItemUI == null)
                        {
                            SpawnNewItem(slot1.currentItemUI.item, slot2);
                        }
                        else
                        {
                            slot2.currentItemUI.item = slot1.currentItemUI.item;
                            slot2.currentItemUI.count = slot1.currentItemUI.count;
                            slot2.currentItemUI.RefreshCount();
                        }
                    }
                    else
                    {
                        slot2.RemoveItem();
                    }
                }
            }
        }
    }
}