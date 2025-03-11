using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BobaStop.Items;

namespace BobaStop.Data.Saved
{
    public class ShopManagerData : ScriptableObject
    {
        public List<Resource> shopSelection;
        public List<Resource> shopInventory;
        public int shopReputationLevel;
        public int currentShopExp;
    }
}
