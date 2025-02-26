using System.Collections;
using System.Collections.Generic;
using BobaStop.Interactions;
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
        
        public override void Interact() {
            Debug.Log(gameObject.name + " picked up");
            Destroy(gameObject);
            // TODO
            // add to inventory
            // if inventory full, dont collect
            // else destroy object
            // sound effect when picking up
        }

        public void SetItem(Items.Item newItem) {
            item = newItem;
        }
    }
}
