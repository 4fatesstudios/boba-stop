using System.Collections.Generic;
using UnityEngine;
using BobaStop.Inventory;
using BobaStop.Items;

namespace BobaStop.UI
{
    public class ShopSelectionUI : MonoBehaviour, IManagerUI
    {
        [SerializeField] private GameObject itemSlotPrefab;
        [SerializeField] private Transform itemSlotContainer;
        [SerializeField] private float slotSize = 50.0f;

        private Dictionary<ItemSlotUI, Item> itemSlotDictionary;

        private void Awake()
        {
            itemSlotDictionary = new Dictionary<ItemSlotUI, Item>();
            CreateItemSlots();
        }

        private void CreateItemSlots()
        {
            for (int i = 0; i < itemSlotContainer.childCount; i++)
            {
                GameObject slotObject = Instantiate(itemSlotPrefab, itemSlotContainer);
                ItemSlotUI itemSlotUI = slotObject.GetComponent<ItemSlotUI>();
                itemSlotUI.Initialize(slotSize);
                itemSlotDictionary.Add(itemSlotUI, null);
            }
        }

        public void CheckIfSlotValid(ItemSlotUI itemSlotUI)
        {
            if (itemSlotUI == null) return;
            if (itemSlotDictionary.ContainsKey(itemSlotUI))
            {
                if (itemSlotDictionary[itemSlotUI] == null)
                {
                    Debug.Log("Slot is empty.");
                }
                else
                {
                    Debug.Log("Slot is not empty.");
                }
            }
        }
    }
}