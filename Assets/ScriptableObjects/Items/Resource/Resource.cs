using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Items
{
    public enum ResourceType {
        Topping,
        Base,
        Sweetener,
        Ice
    }
    
    [CreateAssetMenu(menuName = "Item/Resource", fileName = "Resource")]
    public class Resource : Item {
        public ResourceType resourceType;
        public int resourceSellValue;
        public int resourceBuyValue;
        public GameObject itemPrefab;
    }
}
