using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BobaStop.Items;

namespace BobaStop.Data.Saved
{
    [System.Serializable]
    public class ShopManagerData : SaveData {
        [SerializeField] public List<Resource> shopSelection = new() {null, null, null, null};
        [SerializeField] public List<Resource> shopInventory = new() {null, null, null, null};
        [SerializeField] public int shopReputationLevel = 1;
        [SerializeField] public int currentShopExp = 0;
    }
}
