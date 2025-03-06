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
        /// <summary>
        /// TODO
        /// - A shop "promotion" UI that allows player to increase/decrease weight of specific shop score categories
        /// - Make a variable for max toppings (currently 3, could be okay?) 
        /// </summary>
        
        // private ShopInventory shopInventory;
        private List<Resource> shopSelection;
        private List<Resource> defaultShopSelection;
        private List<Resource> shopInventory;
        private int shopSelectionSize;
        private int shopSelectionScore;
        private int shopInventorySize;
        private int ordersGeneratedPotential;
        private bool shopIsRunning;
        
        public override void Start() {
            defaultShopSelection = new List<Resource> {
                Resources.Load<Resource>("Items/Resource/Bases/BlackTea"), // default Base
                Resources.Load<Resource>("Items/Resource/Foams/MilkFoam"), // default Foam
                Resources.Load<Resource>("Items/Resource/Sweeteners/SimpleSyrup"), // default Sweetener
                Resources.Load<Resource>("Items/Resource/Toppings/Boba") // default Topping
            };

            shopSelectionSize = 10; // something to be loaded in later
            shopInventorySize = 30;

            // Initialize shopSelection with defaultShopSelection and fill remaining slots with null
            shopSelection = new List<Resource>(shopSelectionSize);
            shopSelection.AddRange(defaultShopSelection);
            // while (shopSelection.Count < shopSelectionSize) {
            //     shopSelection.Add(null);
            // }

            // Initialize shopInventory with all nulls
            shopInventory = new List<Resource>(new Resource[shopInventorySize]);

            ValidateShopSelection();
        }


        public override void Update() {
            if (shopIsRunning) {
                OnRunShop();
            }
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
            return shopSelectionSize;
        }

        public List<Resource> GetShopSelection() {
            return shopSelection;
        }

        public void SetShopSelection(List<Resource> shopSelection) {
            this.shopSelection = shopSelection;
        }
        
        public void PrintShopSelection() {
            foreach (var resource in shopSelection) {
                Debug.Log(resource);
            }
        }

        private void ValidateShopSelection() {
            void EnsureResource(ResourceType type) {
                if (!shopSelection.Exists(r => r.resourceType == type)) {
                    Resource defaultResource = defaultShopSelection.Find(r => r.resourceType == type);
                    if (defaultResource != null) {
                        if (shopSelection.Count >= shopSelectionSize) {
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
            
            UpdateShopSelectionScore();
        }

        /// <summary>
        /// Uniquely adds a given resource to the Shop's Selection, not allowed to alter while running shop
        /// </summary>
        /// <param name="resource"></param>
        /// <returns>true if resource was successfully added to selection, false otherwise</returns>
        public bool AddToShopSelection(Resource resource) {
            if (shopIsRunning) return false;
            if (shopSelection.Count >= shopSelectionSize) return false;
            if (shopSelection.Contains(resource)) return false;
                
            shopSelection.Add(resource);
            ValidateShopSelection();
            return true;
        }

        public bool RemoveFromShopSelection(Resource resource) {
            if (shopIsRunning) return false; 
            if (!shopSelection.Contains(resource)) return false;

            shopSelection.Remove(resource);
            ValidateShopSelection();
            return true;
        }

        // AI generated - REQUIRES REVIEW
        private void UpdateShopSelectionScore() {
            // Weights for each factor
            float weightCount = 1f;
            float weightRarity = 1f;
            float weightTypeBalance = 1f;
            float weightRarityBalance = 1f;

            int score = 0;

            // 1. Base score: +1 per resource
            int baseScore = Mathf.RoundToInt(shopSelection.Count * weightCount);
            score += baseScore;
            Debug.Log($"Base Score (Count): {baseScore}");

            // 2. Rarity Bonus
            Dictionary<Rarity, int> rarityPoints = new Dictionary<Rarity, int> {
                { Rarity.Special, 1 },
                { Rarity.Premium, 2 },
                { Rarity.Exquisite, 3 }
            };

            int rarityBonusScore = 0;
            foreach (var resource in shopSelection) {
                if (rarityPoints.TryGetValue(resource.itemRarity, out int rarityBonus)) {
                    rarityBonusScore += Mathf.RoundToInt(rarityBonus * weightRarity);
                }
            }
            score += rarityBonusScore;
            Debug.Log($"Rarity Bonus Score: {rarityBonusScore}");

            // 3. Resource Type Balance (0-10 points)
            var typeCounts = shopSelection.GroupBy(r => r.resourceType)
                .Select(g => g.Count())
                .ToList();

            float typeBalanceScore = CalculateBalanceScore(typeCounts);
            score += Mathf.RoundToInt(typeBalanceScore * weightTypeBalance);
            Debug.Log($"Resource Type Balance Score: {typeBalanceScore}");

            // 4. Rarity Spread Balance (0-10 points)
            var consideredRarities = new List<Rarity> { Rarity.Classic, Rarity.Special, Rarity.Premium, Rarity.Exquisite };
            var rarityCounts = consideredRarities.Select(rarity => shopSelection.Count(resource => resource.itemRarity == rarity)).ToList();
            Debug.Log(rarityCounts.ToString());

            float rarityBalanceScore = CalculateBalanceScore(rarityCounts);
            score += Mathf.RoundToInt(rarityBalanceScore * weightRarityBalance);
            Debug.Log($"Rarity Spread Balance Score: {rarityBalanceScore}");

            // Store the final score
            shopSelectionScore = score;
            Debug.Log($"Total Shop Selection Score: {shopSelectionScore}");
        }

        // AI generated - REQUIRES REVIEW
        // Helper function to compute balance score (0-10)
        private float CalculateBalanceScore(List<int> counts) {
            if (counts.Count == 0) return 0;

            float total = counts.Sum();
            if (total == 0) return 0; // No resources, score should be 0.

            float avg = total / counts.Count;
    
            // Calculate variance (higher variance = worse balance)
            float variance = counts.Sum(c => Mathf.Pow(c - avg, 2)) / counts.Count;
    
            // Normalize to a 0-10 scale (higher variance = lower score)
            float balanceScore = Mathf.Clamp(10 - (variance * 2), 0, 10);

            Debug.Log($"Balance Score Calculation: Avg = {avg}, Variance = {variance}, Balance Score = {balanceScore}");
            return balanceScore;
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
            
            // Calculate the number of toppings as a random number between 1 and 3
            int maxNumToppings = Math.Min(toppingResources.Count, 3);
            int numToppings = Random.Range(1, maxNumToppings); // Randomly generate number of toppings (at least the available toppings)

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
