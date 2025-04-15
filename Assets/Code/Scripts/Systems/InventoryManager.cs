using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.Data.Saved;
using BobaStop.Items;
using BobaStop.Systems;
using UnityEngine;

namespace BobaStop.Inventory
{
    public class InventoryManager : MonoBehaviour, IPersistenceData
    {
        private Dictionary<int, int> inventoryLevelSlots = new() {
            {1, 12},
            {2, 18},
            {3, 24},
        };
        
        public void LoadData(SaveData saveData) {
            
        }
        
        public void SaveData(SaveData saveData) {
            
        }
    }
}