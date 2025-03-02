using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using BobaStop.Items;
using BobaStop.Systems;
using BobaStop.Systems.World;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

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

        // AI generated - REQUIRES REVIEW
        public Order GenerateOrder() {
            // Order defaultOrder = new Order {
            //     drinkBase = Resources.Load<Resource>("Items/Resource/Bases/BlackTea"),
            //     drinkFoam = Resources.Load<Resource>("Items/Resource/Foams/MilkFoam"),
            //     drinkSweetener = Resources.Load<Resource>("Items/Resource/Sweeteners/SimpleSyrup"),
            //     drinkToppings = new Resource[] {Resources.Load<Resource>("Items/Resource/Toppings/Boba")}
            // };
            
            ValidateShopSelection();

            // filter resources by type
            var baseResources = shopSelection.Where(r => r.resourceType == ResourceType.Base).ToList();
            var foamResources = shopSelection.Where(r => r.resourceType == ResourceType.Foam).ToList();
            var sweetenerResources = shopSelection.Where(r => r.resourceType == ResourceType.Sweetener).ToList();
            var toppingResources = shopSelection.Where(r => r.resourceType == ResourceType.Topping).ToList();

            // Randomly select one resource from each type
            Resource drinkBase = baseResources[Random.Range(0, baseResources.Count)];
            Resource drinkFoam = foamResources[Random.Range(0, foamResources.Count)];
            Resource drinkSweetener = sweetenerResources[Random.Range(0, sweetenerResources.Count)];
            
            // Calculate the minimum number of toppings as the max between the number of toppings available in the shop selection and a random number between 1 and 3
            int minNumToppings = toppingResources.Count; // Minimum toppings should be the number of available toppings in the shop
            int maxNumToppings = Mathf.Min(minNumToppings, 3); // Limit the number to 3, or the available number
            int numToppings = Random.Range(minNumToppings, maxNumToppings + 1); // Randomly generate number of toppings (at least the available toppings)

            List<Resource> selectedToppings = new List<Resource>();
            
            // Ensure unique toppings by using a HashSet
            HashSet<Resource> uniqueToppings = new HashSet<Resource>();

            while (uniqueToppings.Count < numToppings) {
                Resource randomTopping = toppingResources[Random.Range(0, toppingResources.Count)];
                uniqueToppings.Add(randomTopping);  // HashSet ensures uniqueness
            }

            // Convert the HashSet to a list for the order
            selectedToppings = uniqueToppings.ToList();

            // Create the order
            Order generatedOrder = new Order {
                drinkBase = drinkBase,
                drinkFoam = drinkFoam,
                drinkSweetener = drinkSweetener,
                drinkToppings = selectedToppings.ToArray()
            };

            // Log the generated order for debugging
            Debug.Log($"Generated Order: {generatedOrder.drinkBase.itemName}, {generatedOrder.drinkFoam.itemName}, {generatedOrder.drinkSweetener.itemName}, Toppings: {string.Join(", ", generatedOrder.drinkToppings.Select(t => t.itemName))}");
            return generatedOrder;
        }

        
        public struct Order {
            private Resource _drinkBase;
            private Resource _drinkFoam;
            private Resource _drinkSweetener;
            private Resource[] _drinkToppings;

            public Resource drinkBase {
                get => _drinkBase;
                set {
                    if (value.resourceType == ResourceType.Base) {
                        _drinkBase = value;
                    } else {
                        Debug.LogError("Invalid Resource type for drinkBase. Must be of type Base.");
                    }
                }
            }

            public Resource drinkFoam {
                get => _drinkFoam;
                set {
                    if (value.resourceType == ResourceType.Foam) {
                        _drinkFoam = value;
                    } else {
                        Debug.LogError("Invalid Resource type for drinkFoam. Must be of type Foam.");
                    }
                }
            }

            public Resource drinkSweetener {
                get => _drinkSweetener;
                set {
                    if (value.resourceType == ResourceType.Sweetener) {
                        _drinkSweetener = value;
                    } else {
                        Debug.LogError("Invalid Resource type for drinkSweetener. Must be of type Sweetener.");
                    }
                }
            }

            public Resource[] drinkToppings {
                get => _drinkToppings;
                set {
                    // Check if all resources in the array are of type Topping
                    if (value.All(r => r.resourceType == ResourceType.Topping)) {
                        _drinkToppings = value;
                    } else {
                        Debug.LogError("Invalid Resource type for drinkToppings. All must be of type Topping.");
                    }
                }
            }
        }
        
    }
}
