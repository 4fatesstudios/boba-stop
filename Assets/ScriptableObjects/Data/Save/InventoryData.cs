using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BobaStop.Items;

namespace BobaStop.Data.Saved
{
    [System.Serializable]
    public class InventoryData : SaveData
    {
        [SerializeField] public ItemSlotContainer<Item> inventory;
        [SerializeField] public ItemSlotContainer<Gear> gear = new(1);
        [SerializeField] public ItemSlotContainer<Weapon> weapon = new(1);

        [SerializeField] public int inventoryLevel = 1;

        public InventoryData() {
            
        }
    }
}
