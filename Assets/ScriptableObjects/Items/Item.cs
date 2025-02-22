using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Items {
    
    public enum Rarity {
        None,
        Seasonal,
        Classic,
        Special,
        Premium,
        Exquisite
    }
    
    public abstract class Item : ScriptableObject {
        public string itemName;
        public string itemDescription;
        public Sprite itemSprite;
        public bool itemStackable = true;
        public Rarity itemRarity;
    }
}