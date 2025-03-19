using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BobaStop.Items;

namespace BobaStop
{
    public class ItemSlot<T> where T : Item {
        private T item;
        private int quantity;
        private int maxQuantity;
        private static int MAX_STACKABLE_QUANTITY = 64;

        public ItemSlot() {
            item = null;
            quantity = 0;
        }
        
        public ItemSlot(T item, int quantity) {
            SetItem(item, quantity);
        }

        public ItemSlot(int maxQuantity) {
            this.maxQuantity = maxQuantity;
        }

        public Item GetItem() {
            return item;
        }

        public int GetQuantity() {
            return quantity;
        }
        
        /// <summary>
        /// clones a single copy of Item into Slot
        /// </summary>
        /// <param name="item">Item to be cloned into Slot</param>
        /// <param name="replace">replace any Item that may exist in Slot already</param>
        /// <returns>true if successfully cloned into Slot, false otherwise</returns>
        public bool CloneItem(T item, bool replace) {
            if (!IsEmpty() && !replace) return false;

            this.item = item;
            quantity = 1;
            return true;
        }

        public bool SetItem(T item, int quantity) {
            if (item == null) {
                this.item = null;
                this.quantity = 0;
                return false;
            }
            
            this.item = item;
            this.quantity = quantity;

            maxQuantity = item.itemStackable ? MAX_STACKABLE_QUANTITY : 1;

            if (quantity > maxQuantity) {
                quantity = maxQuantity;
            }

            return true;
        }

        public bool IsEmpty() {
            return item == null;
        }

        public bool IsFull() {
            return quantity >= maxQuantity;
        }
        
        /// <summary>
        /// Increases quantity of item slot
        /// </summary>
        /// <param name="quantity">amount to add to item slot</param>
        /// <returns>remainder of quantity after adding, -1 if slot is empty</returns>
        public int AddToStack(int quantity) {
            if (IsEmpty()) return -1;

            this.quantity += quantity;
            
            if (this.quantity <= maxQuantity) return 0;

            this.quantity = maxQuantity;
            var remainder = quantity - maxQuantity;
            return remainder;
        }
        
        /// <summary>
        /// Attempts to swap this itemSlot with given itemSlot
        /// </summary>
        /// <param name="itemSlot">itemSlot to attempt swap with</param>
        /// <returns>true if swap successful, false otherwise</returns>
        public bool SwapItemSlots(ItemSlot<T> itemSlot) {
            if (itemSlot.item.GetType() != this.item.GetType()) return false;

            if (itemSlot.IsEmpty() && this.IsEmpty()) return false;

            if (itemSlot.IsEmpty() && !this.IsEmpty()) {
                itemSlot.item = this.item;
                itemSlot.quantity = this.quantity;
                this.ClearSlot();
                return true;
            }

            if (!itemSlot.IsEmpty() && this.IsEmpty()) {
                this.item = itemSlot.item;
                this.quantity = itemSlot.quantity;
                itemSlot.ClearSlot();
                return true;
            }
            
            var tmpItem = this.item;
            var tmpQuantity = this.quantity;
            this.item = itemSlot.item;
            this.quantity = itemSlot.quantity;
            itemSlot.item = tmpItem;
            itemSlot.quantity = tmpQuantity;
            return true;
        }

        public void ClearSlot() {
            item = null;
            quantity = 0;
        }
    }

    public class ItemSlotContainer<T> where T : Item {
        private List<ItemSlot<T>> itemSlots;
        private int slots;
        
        public ItemSlotContainer() {
            itemSlots = new();
            slots = 0;
        }

        public ItemSlotContainer(int slots) {
            itemSlots = new();
            this.slots = slots;
            for (int i = 0; i < slots; i++) {
                itemSlots.Add(new ItemSlot<T>());
            }
        }

        public ItemSlotContainer(ItemSlotContainer<T> container) {
            itemSlots = new(container.itemSlots);
            slots = container.slots;
        }

        public List<ItemSlot<T>> GetItemSlots() {
            return itemSlots;
        }

        public int GetSlots() {
            return slots;
        }

        public void AddSlots(int quantity) {
            slots += quantity;
            for (int i = itemSlots.Count; i < slots; i++) {
                itemSlots.Add(new ItemSlot<T>());
            }
        }

        public void RemoveSlots(int quantity) {
            slots = Mathf.Max(0, slots - quantity);
            for (int i = itemSlots.Count - 1; i >= slots; i--) {
                itemSlots.RemoveAt(i);
            }
        }
    }
}
