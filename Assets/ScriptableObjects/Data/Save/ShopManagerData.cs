using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BobaStop.Items;

namespace BobaStop.Data.Saved
{
    public class ShopManagerData : ScriptableObject
    {
        public List<Resource> shopSelection = new();
        public List<Resource> shopInventory = new();
        public int shopReputationLevel = 1;
        public int currentShopExp = 0;
    }
}
