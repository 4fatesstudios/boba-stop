using System;
using BobaStop.Characters;
using UnityEngine;
using UnityEngine.UIElements;

namespace BobaStop.Inventory
{
    public class InventoryToggle : MonoBehaviour
    {
        [SerializeField] private UIDocument inventoryUIDocument;
        [SerializeField] private Player player;

        private VisualElement inventoryRoot;
        private VisualElement darkBackground;
        private VisualElement mainInventory;

        public static bool isInventoryVisible = false;

        private void Start() {
            Player.Instance.OnOpenInventory += ToggleInventory;
            
            if (inventoryUIDocument != null)
            {
                inventoryRoot = inventoryUIDocument.rootVisualElement;
                
                darkBackground = inventoryRoot.Q<VisualElement>("Container");
                mainInventory = inventoryRoot.Q<VisualElement>("Inventory");
                
                SetInventoryVisibility(false);
            }
        }

        private void OnDestroy() {
            Player.Instance.OnOpenInventory -= ToggleInventory;
            Debug.Log("destroyed");
        }

        private void ToggleInventory(object sender, EventArgs e)
        {
            isInventoryVisible = !isInventoryVisible;
            SetInventoryVisibility(isInventoryVisible);
            
            player.enabled = !isInventoryVisible;
        }
        
        private void SetInventoryVisibility(bool visible)
        {
            if (inventoryRoot == null) return;
            
            DisplayStyle displayStyle = visible ? DisplayStyle.Flex : DisplayStyle.None;
            
            if (darkBackground != null)
            {
                darkBackground.style.display = displayStyle;
            }
            else
            {
                inventoryRoot.style.display = displayStyle;
            }
            
            if (mainInventory != null) mainInventory.style.display = displayStyle;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.I))
            {
                ToggleInventory(this, EventArgs.Empty);
            }
        }
    }
}