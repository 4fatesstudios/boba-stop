using System;
using BobaStop.Characters;
using UnityEngine;

namespace BobaStop.Inventory
{
    public class InventoryToggle : MonoBehaviour
    {
        [SerializeField] private GameObject toolbar;
        [SerializeField] private GameObject darkBackground;
        [SerializeField] private GameObject mainInventory;
        [SerializeField] private GameObject shopSelection;
        [SerializeField] private GameObject shopInventory;
        [SerializeField] private Player player;

        public static bool isInventoryVisible = false;

        private void Start() {
            Player.Instance.OnOpenInventory += ToggleInventory;
            
            // only toolbar is visible at start
            toolbar.SetActive(true);
            darkBackground.SetActive(false);
            mainInventory.SetActive(false);
            shopSelection.SetActive(false);
            shopInventory.SetActive(false);
        }

        private void OnDestroy() {
            Player.Instance.OnOpenInventory -= ToggleInventory;
            Debug.Log("destroyed");
        }

        private void ToggleInventory(object sender, EventArgs e)
        {
            isInventoryVisible = !isInventoryVisible;
            
            toolbar.SetActive(!isInventoryVisible);
            darkBackground.SetActive(isInventoryVisible);
            mainInventory.SetActive(isInventoryVisible);
            shopSelection.SetActive(isInventoryVisible);
            shopInventory.SetActive(isInventoryVisible);
            
            // disable player movement when inventory is visible
            player.enabled = !isInventoryVisible;
        }
    }
}