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

        public static bool operator==(Item a, Item b) {
            return a?.itemName == b?.itemName;
        }

        public static bool operator !=(Item a, Item b) {
            return a?.itemName != b?.itemName;
        }
    }
}