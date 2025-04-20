using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.Inventory;
using UnityEngine;

namespace BobaStop.Items {
    
    public enum Rarity {
        None,
        // Seasonal,
        Classic,
        Special,
        Premium,
        Exquisite
    }
    
    public class Item : ScriptableObject
    {
        // public int id;
        public string ID = Guid.NewGuid().ToString();
        public string itemName;
        public string itemDescription;
        public Sprite itemSprite;
        public bool itemStackable = true;
        public Rarity itemRarity;
        public ItemUI.Dimensions slotDimension;

        public static bool operator==(Item a, Item b) {
            return a?.itemName == b?.itemName;
        }

        public static bool operator !=(Item a, Item b) {
            return a?.itemName != b?.itemName;
        }

        public override bool Equals(object obj) {
            if (obj is Item item) {
                return this == item;
            }
            return false;
        }

        public override int GetHashCode() {
            return itemName?.GetHashCode() ?? 0;
        }
    }
}