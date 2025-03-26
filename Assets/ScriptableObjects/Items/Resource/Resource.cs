using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Items
{
    public enum ResourceType {
        Topping, // boba, jelly, agar, taro, etc
        Base, // green tea, osmanthus, high mountain, oolong, black, etc
        Sweetener, // syrup, honey, condensed milk, etc
        Foam // milk foam, cheese foam, sweet and salty mousse, tiramisu mousse, macha foam, etc
    }
    
    [CreateAssetMenu(menuName = "Item/Resource", fileName = "Resource")]
    public class Resource : Item {
        public ResourceType resourceType;
        public int resourceSellValue;
        [NonSerialized] public int resourceShopSellValue;
        public int resourceBuyValue;
        public GameObject itemPrefab;
        private const float SHOP_SELL_VALUE_MULTIPLIER = 1.6f;

        public void OnEnable() {
            resourceShopSellValue = Mathf.CeilToInt(resourceSellValue * SHOP_SELL_VALUE_MULTIPLIER);
        }
    }
}
