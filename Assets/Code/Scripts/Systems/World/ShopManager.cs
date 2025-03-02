using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using BobaStop.Items;
using BobaStop.Systems;
using BobaStop.Systems.World;
using UnityEngine;

namespace BobaStop.Systems.World
{
    public class ShopManager : WorldSystemManager
    {
        // private ShopInventory shopInventory;
        private List<Resource> shopSelection;
        private List<Resource> defaultShopSelection;
        private int maxShopSelectionSize;
        private int shopSelectionScore;
        private int ordersGeneratedPotential;
        private bool shopIsRunning;

        public override void Start() {
            defaultShopSelection = new List<Resource> {
                Resources.Load<Resource>("Items/Resource/Bases/BlackTea"), // default Base
                Resources.Load<Resource>("Items/Resource/Foams/MilkFoam"), // default Foam
                Resources.Load<Resource>("Items/Resource/Sweeteners/SimpleSyrup"), // default Sweetener
                Resources.Load<Resource>("Items/Resource/Toppings/Boba") // default Topping
            };
            shopSelection = new List<Resource>(defaultShopSelection); // will be overwritten by save

            maxShopSelectionSize = 10; // something to be loaded in later
        }

        public override void Update() {
            if (shopIsRunning) {
                OnRunShop();
            }
        }

        public override bool LoadData() {
            return true;
        }

        public void StartShopDay() {
            shopIsRunning = true;
        }

        public void EndShopDay() {
            shopIsRunning = false;
        }

        private void OnRunShop() {
            
        }

        public int GetShopSelectionScore() {
            return shopSelectionScore;
        }

        public int GetMaxShopSelectionSize() {
            return maxShopSelectionSize;
        }

        public List<Resource> GetShopSelection() {
            return shopSelection;
        }

        private void ValidateShopSelection() {
            void EnsureResource(ResourceType type) {
                if (!shopSelection.Exists(r => r.resourceType == type)) {
                    Resource defaultResource = defaultShopSelection.Find(r => r.resourceType == type);
                    if (defaultResource != null) {
                        if (shopSelection.Count >= maxShopSelectionSize) {
                            shopSelection.RemoveAt(shopSelection.Count - 1); // Remove last added
                        }
                        shopSelection.Add(defaultResource);
                    }
                }
            }

            EnsureResource(ResourceType.Topping);
            EnsureResource(ResourceType.Base);
            EnsureResource(ResourceType.Foam);
            EnsureResource(ResourceType.Sweetener);
        }

        /// <summary>
        /// Uniquely adds a given resource to the Shop's Selection, not allowed to alter while running shop
        /// </summary>
        /// <param name="resource"></param>
        /// <returns>true if resource was successfully added to selection, false otherwise</returns>
        public bool AddToShopSelection(Resource resource) {
            if (shopIsRunning) return false;
            if (shopSelection.Count >= maxShopSelectionSize) return false;
            if (shopSelection.Contains(resource)) return false;
                
            shopSelection.Add(resource);
            ValidateShopSelection();
            UpdateShopSelectionScore();
            return true;
        }

        public bool RemoveFromShopSelection(Resource resource) {
            if (shopIsRunning) return false; 
            if (!shopSelection.Contains(resource)) return false;

            shopSelection.Remove(resource);
            ValidateShopSelection();
            UpdateShopSelectionScore();
            return true;
        }

        private void UpdateShopSelectionScore() {
            //
        }

        private void GenerateOrder() {
            //
        }
    }
}
