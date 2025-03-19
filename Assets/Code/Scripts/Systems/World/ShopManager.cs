using System;
using System.Collections.Generic;
using System.Linq;
using BobaStop.Data;
using BobaStop.Items;
using BobaStop.Data.Saved;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BobaStop.Systems.World
{
    public class ShopManager : RealtimeSystemManager, IPersistenceData, IScheduledObject
    {
        /// <summary>
        /// TODO
        /// - Delegate ShopManagerData data management to a separate "ShopManagerDataManager"
        /// - A shop "promotion" UI that allows player to increase/decrease weight of specific shop score categories
        /// - Make a variable for max toppings (currently 3, could be okay?)
        /// </summary>
        
        private List<Resource> defaultShopSelection;
        private int shopSelectionScore;
        private bool shopIsRunning;
        private ShopReputation shopReputation;
        private bool isScheduledTime;
        private ShopReputationData shopReputationData = Resources.Load<ShopReputationData>("Data/ShopReputationData");
        private ShopManagerData shopManagerData = new();
        

        public override void Start() {
            defaultShopSelection = new List<Resource> {
                Resources.Load<Resource>("Items/Resource/Bases/BlackTea"), // default Base
                Resources.Load<Resource>("Items/Resource/Foams/MilkFoam"), // default Foam
                Resources.Load<Resource>("Items/Resource/Sweeteners/SimpleSyrup"), // default Sweetener
                Resources.Load<Resource>("Items/Resource/Toppings/Boba") // default Topping
            };
            
            // add to schedule manager
            GameManager.Instance.scheduleManager.AddToSchedule(this);
        }

        public override void Update() {
            // if (shopIsRunning) {
            //     OnRunShop();
            // }

            if (isScheduledTime) {
                OnRunShop();
            }
        }
        
        #region IPersistenceData
        public void LoadData(SaveData saveData) {
            if (saveData is not ShopManagerData shopManagerData) {
                Debug.LogWarning("Save data is not a ShopManagerData");
                return;
            }
            
            this.shopManagerData.shopSelection = new List<Resource>(shopManagerData.shopSelection);
            this.shopManagerData.shopInventory = new List<Resource>(shopManagerData.shopInventory);
            this.shopManagerData.currentShopExp = shopManagerData.currentShopExp;
            
            this.shopManagerData.shopReputationLevel = shopManagerData.shopReputationLevel;
            shopReputation = shopReputationData.GetReputation(this.shopManagerData.shopReputationLevel);

            this.shopManagerData.schedule = new Schedule(shopManagerData.schedule);
            
            ValidateShopSelection();
        }

        public void SaveData(SaveData saveData) {
            if (saveData is not ShopManagerData shopManagerData) {
                Debug.LogWarning("Save data is not a ShopManagerData");
                return;
            }
            
            shopManagerData.shopSelection = new List<Resource>(this.shopManagerData.shopSelection);
            shopManagerData.shopInventory = new List<Resource>(this.shopManagerData.shopInventory);
            shopManagerData.currentShopExp = this.shopManagerData.currentShopExp;
            shopManagerData.shopReputationLevel = this.shopManagerData.shopReputationLevel;
            shopManagerData.schedule = new Schedule(this.shopManagerData.schedule);
        }
        #endregion
        
        #region IScheduledObject

        public Schedule GetSchedule() {
            return shopManagerData.schedule;
        }

        public void SetIsScheduledTime(bool isScheduledTime) {
            this.isScheduledTime = isScheduledTime;
        }

        public bool GetIsScheduledTime() {
            return isScheduledTime;
        }
        
        #endregion

        public void IncreaseShopReputationLevel() {
            ++shopManagerData.shopReputationLevel;
            shopReputation = shopReputationData.GetReputation(shopManagerData.shopReputationLevel);
            while (shopManagerData.shopSelection.Count < shopReputation.shopSelectionSize)
                shopManagerData.shopSelection.Add(null);
            while (shopManagerData.shopInventory.Count < shopReputation.shopInventorySize)
                shopManagerData.shopInventory.Add(null);
        }
        
        private void UpdateShopReputationLevel() {
            if (shopManagerData.currentShopExp < shopReputation.expToLevel) return;
            
            ++shopManagerData.shopReputationLevel;
            shopReputation = shopReputationData.GetReputation(shopManagerData.shopReputationLevel);
            while (shopManagerData.shopSelection.Count < shopReputation.shopSelectionSize)
                shopManagerData.shopSelection.Add(null);
            while (shopManagerData.shopInventory.Count < shopReputation.shopInventorySize)
                shopManagerData.shopInventory.Add(null);
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
            return shopReputation.shopSelectionSize;
        }

        public List<Resource> GetShopSelection() {
            return shopManagerData.shopSelection;
        }

        public void SetShopSelection(List<Resource> shopSelection) {
            shopManagerData.shopSelection = shopSelection;
        }
        
        public void PrintShopSelection() {
            foreach (var resource in shopManagerData.shopSelection) {
                Debug.Log(resource);
            }
        }

        private void ValidateShopSelection() {
            // Ensure there is still at least 1 of each resource type after potential null addition
            void EnsureResource(ResourceType type) {
                if (shopManagerData.shopSelection.All(r => r == null || r.resourceType != type)) {
                    Resource defaultResource = defaultShopSelection.Find(r => r.resourceType == type);
                    if (defaultResource != null) {
                        // Add the default resource to the first available empty slot (null)
                        for (int i = 0; i < shopManagerData.shopSelection.Count; i++) {
                            if (shopManagerData.shopSelection[i] == null) {
                                shopManagerData.shopSelection[i] = defaultResource;
                                return;
                            }
                        }
                        Debug.LogError("No empty slot found in shopSelection to add default resource.");
                    }
                }
            }

            EnsureResource(ResourceType.Topping);
            EnsureResource(ResourceType.Base);
            EnsureResource(ResourceType.Foam);
            EnsureResource(ResourceType.Sweetener);
            
            UpdateShopSelectionScore();
        }
        
        public bool AddToShopSelection(Resource resource) {
            if (shopIsRunning) return false;
            if (shopManagerData.shopSelection.Contains(resource)) return false;

            for (int i = 0; i < shopManagerData.shopSelection.Count; ++i) {
                if (shopManagerData.shopSelection[i] == null) {
                    shopManagerData.shopSelection[i] = resource;
                    ValidateShopSelection();
                    return true;
                }
            }
            
            // if not available spot found, do not add
            ValidateShopSelection();
            return false;
        }
        
        public bool AddToShopSelection(Resource resource, int index) {
            if (shopIsRunning) return false;
            if (shopManagerData.shopSelection.Contains(resource)) return false;
            if (index < 0 || index >= shopManagerData.shopSelection.Count) return false;

            shopManagerData.shopSelection[index] = resource;
            
            ValidateShopSelection();
            return true;
        }

        public bool RemoveFromShopSelection(Resource resource) {
            if (shopIsRunning) return false; 
            if (!shopManagerData.shopSelection.Contains(resource)) return false;
            
            shopManagerData.shopSelection[shopManagerData.shopSelection.IndexOf(resource)] = null;
            ValidateShopSelection();
            return true;
        }

        public bool RemoveFromShopSelection(int index) {
            if (shopIsRunning) return false;
            if (index < 0 || index >= shopManagerData.shopSelection.Count) return false;

            shopManagerData.shopSelection[index] = null;
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
            int baseScore = Mathf.RoundToInt(shopManagerData.shopSelection.Count(resource => resource != null) * weightCount);
            score += baseScore;
            Debug.Log($"Base Score (Count): {baseScore}");

            // 2. Rarity Bonus
            Dictionary<Rarity, int> rarityPoints = new Dictionary<Rarity, int> {
                { Rarity.Special, 1 },
                { Rarity.Premium, 2 },
                { Rarity.Exquisite, 3 }
            };

            int rarityBonusScore = 0;
            foreach (var resource in shopManagerData.shopSelection) {
                if (resource != null) {
                    if (rarityPoints.TryGetValue(resource.itemRarity, out int rarityBonus)) {
                        rarityBonusScore += Mathf.RoundToInt(rarityBonus * weightRarity);
                    }
                }
            }
            score += rarityBonusScore;
            Debug.Log($"Rarity Bonus Score: {rarityBonusScore}");

            // 3. Resource Type Balance (0-10 points)
            var typeCounts = shopManagerData.shopSelection
                .Where(r => r != null) // Filter out nulls
                .GroupBy(r => r.resourceType)
                .Select(g => g.Count())
                .ToList();

            float typeBalanceScore = CalculateBalanceScore(typeCounts);
            score += Mathf.RoundToInt(typeBalanceScore * weightTypeBalance);
            Debug.Log($"Resource Type Balance Score: {typeBalanceScore}");

            // 4. Rarity Spread Balance (0-10 points)
            var consideredRarities = new List<Rarity> { Rarity.Classic, Rarity.Special, Rarity.Premium, Rarity.Exquisite };
            var rarityCounts = consideredRarities.Select(rarity => shopManagerData.shopSelection.Count(resource => resource != null && resource.itemRarity == rarity)).ToList();
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
            ValidateShopSelection();

            // filter resources by type, excluding nulls
            var baseResources = shopManagerData.shopSelection.Where(r => r != null && r.resourceType == ResourceType.Base).ToList();
            var foamResources = shopManagerData.shopSelection.Where(r => r != null && r.resourceType == ResourceType.Foam).ToList();
            var sweetenerResources = shopManagerData.shopSelection.Where(r => r != null && r.resourceType == ResourceType.Sweetener).ToList();
            var toppingResources = shopManagerData.shopSelection.Where(r => r != null && r.resourceType == ResourceType.Topping).ToList();
             
            if (baseResources.Count == 0 || foamResources.Count == 0 || sweetenerResources.Count == 0 || toppingResources.Count == 0)
            {
                Debug.LogError("Not enough resources to generate an order. There needs to be at least one of each type");
                return default;
            }

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

        /// <summary>
        /// Removes the given Order ingredients from inventory if all ingredients are present then returns true,
        /// returns false if not all ingredients available and does not remove any in this case
        /// </summary>
        /// <param name="order">Order containing ingredients to remove from inventory</param>
        /// <returns></returns>
        private bool RemoveFromShopInventory(Order order) {
            return false;
        }
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
