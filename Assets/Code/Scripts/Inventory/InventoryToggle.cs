using System;
using UnityEngine;

namespace BobaStop.Inventory
{
    public class InventoryToggle : MonoBehaviour
    {
        [SerializeField] private GameObject toolbar;
        [SerializeField] private GameObject darkBackground;
        [SerializeField] private GameObject mainInventory;

        private bool isInventoryVisible = false;

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
        }
    }
}