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
        public int resourceBuyValue;
    }
}
