using System.Collections;
using System.Collections.Generic;
using BobaStop.Interactions;
using BobaStop.Inventory;
using BobaStop.Systems;
using UnityEngine;

namespace BobaStop.Interactions
{
    public class InteractablePickupable : Interactable {
        [SerializeField] private Items.Item item;
        [SerializeField] private GameObject selectedVisualGameObject;
        [SerializeField] private GameObject unselectedVisualGameObject;
        
        private void Start() {
            selectedVisualGameObject.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
            unselectedVisualGameObject.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        }
        
        // public override void Interact() {
        //     Debug.Log(gameObject.name + " picked up");
        //     Destroy(gameObject);
        //     // add to inventory
        //     // if inventory full, dont collect
        //     // else destroy object
        //     // sound effect when picking up
        // }

        public override void Interact()
        {
            if (InventoryToggle.isInventoryVisible)
            {
                Debug.Log("Cannot pick up item while inventory is open");
                return;
            }

            var result = GameManager.Instance.inventoryManager.AddItemToInventory(item, 1);
            Debug.Log(result);
            if (result == 0) Destroy(gameObject);
            else Debug.Log("Cannot pick up item, inventory full");
        }

        public void SetItem(Items.Item newItem) {
            item = newItem;
        }
    }
}
