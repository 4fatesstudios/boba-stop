using System.Collections.Generic;
using UnityEngine;
using BobaStop.Inventory;
using BobaStop.Items;

namespace BobaStop.UI
{
    public class InventoryUIManager : MonoBehaviour, IManagerUI
    {
        [SerializeField] private GameObject itemSlotPrefab;
        [SerializeField] private Transform itemSlotContainer;
        [SerializeField] private float slotSize = 50.0f;

        private InventoryManager inventoryManager;

        private void Awake()
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            inventoryManager.InitializeSlots(itemSlotContainer.childCount, itemSlotPrefab, itemSlotContainer, slotSize);
        }

        public void CheckIfSlotValid(ItemSlotUI itemSlotUI)
        {
            if (itemSlotUI == null) return;
            if (inventoryManager.IsSlotValid(itemSlotUI))
            {
                if (inventoryManager.IsSlotEmpty(itemSlotUI))
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