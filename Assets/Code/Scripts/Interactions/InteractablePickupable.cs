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

        private void Awake() {
            if (item == null) {
                Debug.LogWarning(gameObject.name + " item is null");
            }
        }
        private void Start() {
            selectedVisualGameObject.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
            unselectedVisualGameObject.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        }
        
        public override void Interact() {
            // TODO
            // add to inventory
            // if inventory full, dont collect
            // else destroy object
            // sound effect when picking up
        }
    }
}
