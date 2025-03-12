using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BobaStop.Items;

namespace BobaStop.Data.Saved
{
    [CreateAssetMenu(menuName = "Data/Saved Data/World/Shop Manager", fileName = "ShopManagerSavedData")]
    public class ShopManagerData : SaveData {
        [SerializeField] public List<Resource> shopSelection = new();
        [SerializeField] public List<Resource> shopInventory = new();
        [SerializeField] public int shopReputationLevel = 1;
        [SerializeField] public int currentShopExp = 0;

        public override void CopyFrom(SaveData other) {
            if (other is not ShopManagerData data) return;
    
            // copy lists by creating new ones and adding items.
            this.shopSelection = new List<Resource>(data.shopSelection);
            this.shopInventory = new List<Resource>(data.shopInventory);

            // directly copy primitive types
            this.shopReputationLevel = data.shopReputationLevel;
            this.currentShopExp = data.currentShopExp;
        }
    }
}
