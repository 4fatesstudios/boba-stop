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
        [SerializeField] private Player player;

        public static bool isInventoryVisible = false;

        private void Start()
        {
            // only toolbar is visible at start
            toolbar.SetActive(true);
            darkBackground.SetActive(false);
            mainInventory.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleInventory();
            }
        }
        
        private void ToggleInventory()
        {
            isInventoryVisible = !isInventoryVisible;
            
            toolbar.SetActive(!isInventoryVisible);
            darkBackground.SetActive(isInventoryVisible);
            mainInventory.SetActive(isInventoryVisible);
            
            // disable player movement when inventory is visible
            player.enabled = !isInventoryVisible;
        }
    }
}