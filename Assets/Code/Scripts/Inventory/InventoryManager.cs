using System.Collections.Generic;
using BobaStop.Items;
using UnityEngine;

namespace BobaStop.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance; // Singleton for easy access

        public List<InventorySlot> slots; // List of all inventory slots
        private Dictionary<string, Item> itemDictionary; // Dictionary to store items by name

        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Make persistent across scenes
            }
            else
            {
                Destroy(gameObject);
            }

            // Initialize the item dictionary
            itemDictionary = new Dictionary<string, Item>();
            PopulateItemDictionary();
        }

        // Populate the dictionary with all Item assets
        private void PopulateItemDictionary()
        {
            // Load all Item assets from the project
            Item[] allItems = Resources.LoadAll<Item>("");
            foreach (Item item in allItems)
            {
                itemDictionary[item.itemName] = item;
            }
        }

        // Add an item to the inventory
        public bool AddItem(Item item)
        {
            // Find an empty slot
            InventorySlot emptySlot = FindEmptySlot();

            if (emptySlot != null)
            {
                // Get the prefab based on the item type
                GameObject prefab = GetItemPrefab(item);

                if (prefab != null)
                {
                    // Instantiate the item UI and place it in the slot
                    GameObject itemObject = Instantiate(prefab, emptySlot.transform);
                    InventoryItem inventoryItem = itemObject.GetComponent<InventoryItem>();
                    inventoryItem.item = item;
                    emptySlot.currentItem = inventoryItem;
                    return true; // Item added successfully
                }
                else
                {
                    Debug.LogError($"Item prefab is missing for: {item.itemName}");
                    return false;
                }
            }

            Debug.Log("No empty slots available!");
            return false; // Inventory is full
        }

        // Get the prefab for an item based on its type
        private GameObject GetItemPrefab(Item item)
        {
            if (item is Weapon weapon)
            {
                return weapon.itemPrefab;
            }
            else if (item is Gear gear)
            {
                return gear.itemPrefab;
            }
            else if (item is Resource resource)
            {
                return resource.itemPrefab;
            }
            else
            {
                Debug.LogError($"Unknown item type: {item.GetType()}");
                return null;
            }
        }

        // Remove an item from the inventory
        public void RemoveItem(Item item)
        {
            // Find the slot containing the item
            InventorySlot slot = slots.Find(s => s.currentItem != null && s.currentItem.item == item);

            if (slot != null)
            {
                Destroy(slot.currentItem.gameObject); // Destroy the item UI
                slot.RemoveItem(); // Clear the slot
            }
            else
            {
                Debug.Log("Item not found in inventory!");
            }
        }

        // Find an empty slot
        public InventorySlot FindEmptySlot()
        {
            return slots.Find(slot => slot.currentItem == null);
        }

        // Check if the inventory is full
        public bool IsInventoryFull()
        {
            return slots.TrueForAll(slot => slot.currentItem != null);
        }

        // Swap items between two slots
        public void SwapItems(InventorySlot slot1, InventorySlot slot2)
        {
            if (slot1.currentItem != null && slot2.currentItem != null)
            {
                // Swap the items
                InventoryItem tempItem = slot1.currentItem;
                slot1.currentItem = slot2.currentItem;
                slot2.currentItem = tempItem;

                // Update the parent transforms
                slot1.currentItem.transform.SetParent(slot1.transform);
                slot2.currentItem.transform.SetParent(slot2.transform);

                // Reset positions
                slot1.currentItem.transform.localPosition = Vector3.zero;
                slot2.currentItem.transform.localPosition = Vector3.zero;
            }
        }

        // Save the inventory state
        public void SaveInventory()
        {
            List<ItemData> itemDataList = new List<ItemData>();

            foreach (InventorySlot slot in slots)
            {
                if (slot.currentItem != null)
                {
                    itemDataList.Add(new ItemData(slot.currentItem.item));
                }
            }

            string json = JsonUtility.ToJson(new InventoryData(itemDataList), true);
            PlayerPrefs.SetString("Inventory", json);
            Debug.Log("Inventory saved!");
        }

        // Load the inventory state
        public void LoadInventory()
        {
            if (PlayerPrefs.HasKey("Inventory"))
            {
                string json = PlayerPrefs.GetString("Inventory");
                InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(json);

                // Clear existing items in the inventory
                foreach (InventorySlot slot in slots)
                {
                    if (slot.currentItem != null)
                    {
                        Destroy(slot.currentItem.gameObject);
                        slot.RemoveItem();
                    }
                }

                // Load items from the saved data
                foreach (ItemData itemData in inventoryData.items)
                {
                    Item item = GetItemByName(itemData.itemName); // Look up the item by name
                    if (item != null)
                    {
                        AddItem(item);
                    }
                    else
                    {
                        Debug.LogError($"Item not found: {itemData.itemName}");
                    }
                }

                Debug.Log("Inventory loaded!");
            }
            else
            {
                Debug.Log("No saved inventory found!");
            }
        }

        // Get an item by name from the dictionary
        public Item GetItemByName(string itemName)
        {
            if (itemDictionary.ContainsKey(itemName))
            {
                return itemDictionary[itemName];
            }
            else
            {
                Debug.LogError($"Item not found: {itemName}");
                return null;
            }
        }
    }

    // Helper class for saving inventory data
    [System.Serializable]
    public class InventoryData
    {
        public List<ItemData> items;

        public InventoryData(List<ItemData> items)
        {
            this.items = items;
        }
    }

    // Helper class for saving item data
    [System.Serializable]
    public class ItemData
    {
        public string itemName; // Name of the item

        public ItemData(Item item)
        {
            this.itemName = item.itemName;
        }
    }
}