using System.Collections.Generic;
using BobaStop.Items;
using BobaStop.UI;
using UnityEngine;

namespace BobaStop.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public int maxStackedItems = 10;
        public ItemSlotUI[] inventorySlots;
        public GameObject inventoryItemPrefab;
        public List<Item> items;

        public ItemSlotUI[] toolbar1Slots;
        public ItemSlotUI[] toolbar2Slots;

        private Dictionary<ItemSlotUI, Item> itemSlotDictionary;

        private void Awake()
        {
            itemSlotDictionary = new Dictionary<ItemSlotUI, Item>();
            InitializeSlots(inventorySlots.Length, inventoryItemPrefab, transform, 50.0f);
        }

        public void InitializeSlots(int slotCount, GameObject itemSlotPrefab, Transform itemSlotContainer, float slotSize)
        {
            for (int i = 0; i < slotCount; i++)
            {
                GameObject slotObject = Instantiate(itemSlotPrefab, itemSlotContainer);
                ItemSlotUI itemSlotUI = slotObject.GetComponent<ItemSlotUI>();
                itemSlotUI.Initialize(slotSize);
                itemSlotDictionary.Add(itemSlotUI, null);
            }
        }

        public bool IsSlotValid(ItemSlotUI itemSlotUI)
        {
            return itemSlotDictionary.ContainsKey(itemSlotUI);
        }

        public bool IsSlotEmpty(ItemSlotUI itemSlotUI)
        {
            return itemSlotDictionary[itemSlotUI] == null;
        }
    }
}