using System.Collections.Generic;
using UnityEngine;
using BobaStop.Inventory;
using BobaStop.Items;
using BobaStop.Systems.World;

namespace BobaStop.UI
{
    public class ShopManagerUI : MonoBehaviour, IManagerUI
    {
        [SerializeField] private GameObject itemSlotPrefab;
        [SerializeField] private Transform itemSlotContainer;
        [SerializeField] private float slotSize = 50.0f;

        private ShopManager shopManager;

        private void Awake()
        {
            shopManager = new ShopManager();
            shopManager.InitializeSlots(itemSlotContainer.childCount, itemSlotPrefab, itemSlotContainer, slotSize);
        }

        public void CheckIfSlotValid(ItemSlotUI itemSlotUI)
        {
            if (itemSlotUI == null) return;
            if (shopManager.IsSlotValid(itemSlotUI))
            {
                if (shopManager.IsSlotEmpty(itemSlotUI))
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