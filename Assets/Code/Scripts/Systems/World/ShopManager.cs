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
            
            this.shopManagerData.shopSelection = new (shopManagerData.shopSelection);
            this.shopManagerData.shopInventory = new(shopManagerData.shopInventory);
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
            
            shopManagerData.shopSelection = new(this.shopManagerData.shopSelection);
            shopManagerData.shopInventory = new(this.shopManagerData.shopInventory);
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
            while (shopManagerData.shopSelection.GetSlots() < shopReputation.shopSelectionSize)
                shopManagerData.shopSelection.AddSlots(1);
            while (shopManagerData.shopInventory.GetSlots() < shopReputation.shopInventorySize)
                shopManagerData.shopInventory.AddSlots(1);
        }
        
        private void UpdateShopReputationLevel() {
            if (shopManagerData.currentShopExp < shopReputation.expToLevel) return;
            
            ++shopManagerData.shopReputationLevel;
            shopReputation = shopReputationData.GetReputation(shopManagerData.shopReputationLevel);
            while (shopManagerData.shopSelection.GetSlots() < shopReputation.shopSelectionSize)
                shopManagerData.shopSelection.AddSlots(1);
            while (shopManagerData.shopInventory.GetSlots() < shopReputation.shopInventorySize)
                shopManagerData.shopInventory.AddSlots(1);
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

        public ItemSlotContainer<Resource> GetShopSelection() {
            return shopManagerData.shopSelection;
        }

        public void SetShopSelection(ItemSlotContainer<Resource> shopSelection) {
            shopManagerData.shopSelection = shopSelection;
        }
        
        public void PrintShopSelection() {
            foreach (var resource in shopManagerData.shopSelection) {
                Debug.Log(resource.GetItem().itemName);
            }
        }

        private void ValidateShopSelection() {
            // Ensure there is still at least 1 of each resource type after potential null addition
            void EnsureResource(ResourceType type) {
                if (shopManagerData.shopSelection.All(slot => slot.IsEmpty() || ((Resource)slot.GetItem()).resourceType != type)) {
                    Resource defaultResource = defaultShopSelection.Find(r => r.resourceType == type);
                    if (defaultResource != null) {
                        // Add the default resource to the first available empty slot
                        foreach (var slot in shopManagerData.shopSelection) {
                            if (slot.IsEmpty()) {
                                slot.SetItem(defaultResource, 1);
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
            
            foreach (var slot in shopManagerData.shopSelection) {
                if (slot.IsEmpty()) {
                    slot.CloneItem(resource, true);
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
            if (index < 0 || index >= shopManagerData.shopSelection.GetSlots()) return false;

            shopManagerData.shopSelection[index].CloneItem(resource, true);
            ValidateShopSelection();
            return true;
        }

        public bool RemoveFromShopSelection(Resource resource) {
            if (shopIsRunning) return false; 
            if (!shopManagerData.shopSelection.Contains(resource)) return false;
            
            shopManagerData.shopSelection[shopManagerData.shopSelection.IndexOf(resource)].ClearSlot();
            ValidateShopSelection();
            return true;
        }

        public bool RemoveFromShopSelection(int index) {
            if (shopIsRunning) return false;
            if (index < 0 || index >= shopManagerData.shopSelection.GetSlots()) return false;

            shopManagerData.shopSelection[index].ClearSlot();
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
            int baseScore = Mathf.RoundToInt(shopManagerData.shopSelection.Count(slot => !slot.IsEmpty()) * weightCount);
            score += baseScore;
            // Debug.Log($"Base Score (Count): {baseScore}");

            // 2. Rarity Bonus
            Dictionary<Rarity, int> rarityPoints = new Dictionary<Rarity, int> {
                { Rarity.Special, 1 },
                { Rarity.Premium, 2 },
                { Rarity.Exquisite, 3 }
            };

            int rarityBonusScore = 0;
            foreach (var slot in shopManagerData.shopSelection) {
                if (!slot.IsEmpty()) {
                    var resource = (Resource)slot.GetItem(); // Cast to Resource
                    if (rarityPoints.TryGetValue(resource.itemRarity, out int rarityBonus)) {
                        rarityBonusScore += Mathf.RoundToInt(rarityBonus * weightRarity);
                    }
                }
            }
            score += rarityBonusScore;
            // Debug.Log($"Rarity Bonus Score: {rarityBonusScore}");

            // 3. Resource Type Balance (0-10 points)
            var typeCounts = shopManagerData.shopSelection
                .Where(slot => !slot.IsEmpty())
                .Select(slot => ((Resource)slot.GetItem()).resourceType)
                .GroupBy(type => type)
                .Select(group => group.Count())
                .ToList();

            float typeBalanceScore = CalculateBalanceScore(typeCounts);
            score += Mathf.RoundToInt(typeBalanceScore * weightTypeBalance);
            // Debug.Log($"Resource Type Balance Score: {typeBalanceScore}");

            // 4. Rarity Spread Balance (0-10 points)
            var consideredRarities = new List<Rarity> { Rarity.Classic, Rarity.Special, Rarity.Premium, Rarity.Exquisite };
            var rarityCounts = consideredRarities
                .Select(rarity => shopManagerData.shopSelection.Count(slot => !slot.IsEmpty() && ((Resource)slot.GetItem()).itemRarity == rarity))
                .ToList();

            float rarityBalanceScore = CalculateBalanceScore(rarityCounts);
            score += Mathf.RoundToInt(rarityBalanceScore * weightRarityBalance);
            // Debug.Log($"Rarity Spread Balance Score: {rarityBalanceScore}");

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

            // Debug.Log($"Balance Score Calculation: Avg = {avg}, Variance = {variance}, Balance Score = {balanceScore}");
            return balanceScore;
        }
        
        // AI generated - REQUIRES REVIEW
        public Order GenerateOrder() {
            ValidateShopSelection();

            // Filter resources by type, excluding empty slots
            var baseResources = shopManagerData.shopSelection
                .Where(slot => !slot.IsEmpty() && ((Resource)slot.GetItem()).resourceType == ResourceType.Base)
                .Select(slot => (Resource)slot.GetItem())
                .ToList();

            var foamResources = shopManagerData.shopSelection
                .Where(slot => !slot.IsEmpty() && ((Resource)slot.GetItem()).resourceType == ResourceType.Foam)
                .Select(slot => (Resource)slot.GetItem())
                .ToList();

            var sweetenerResources = shopManagerData.shopSelection
                .Where(slot => !slot.IsEmpty() && ((Resource)slot.GetItem()).resourceType == ResourceType.Sweetener)
                .Select(slot => (Resource)slot.GetItem())
                .ToList();

            var toppingResources = shopManagerData.shopSelection
                .Where(slot => !slot.IsEmpty() && ((Resource)slot.GetItem()).resourceType == ResourceType.Topping)
                .Select(slot => (Resource)slot.GetItem())
                .ToList();

            if (baseResources.Count == 0 || foamResources.Count == 0 || sweetenerResources.Count == 0 || toppingResources.Count == 0) {
                Debug.LogError("Not enough resources to generate an order. There needs to be at least one of each type.");
                return default;
            }

            // Randomly select one resource from each type
            Resource drinkBase = baseResources[Random.Range(0, baseResources.Count)];
            Resource drinkFoam = foamResources[Random.Range(0, foamResources.Count)];
            Resource drinkSweetener = sweetenerResources[Random.Range(0, sweetenerResources.Count)];

            // Ensure numToppings is between 1 and maxNumToppings
            int maxNumToppings = Mathf.Min(toppingResources.Count, 3);
            int numToppings = Random.Range(1, maxNumToppings + 1); // Include maxNumToppings

            // Ensure unique toppings using HashSet
            var selectedToppings = new HashSet<Resource>();
            while (selectedToppings.Count < numToppings) {
                selectedToppings.Add(toppingResources[Random.Range(0, toppingResources.Count)]);
            }

            // Create the order
            Order generatedOrder = new Order {
                drinkBase = drinkBase,
                drinkFoam = drinkFoam,
                drinkSweetener = drinkSweetener,
                drinkToppings = selectedToppings.ToArray()
            };

            // Log the generated order
            Debug.Log($"Generated Order: {drinkBase.itemName}, {drinkFoam.itemName}, {drinkSweetener.itemName}, Toppings: {string.Join(", ", generatedOrder.drinkToppings.Select(t => t.itemName))}");

            return generatedOrder;
        }


        /// <summary>
        /// Removes the given Order ingredients from inventory if all ingredients are present
        /// </summary>
        /// <param name="order">Order containing ingredients to remove from inventory</param>
        /// <returns>false if not all ingredients for order found in shopInventory, true otherwise</returns>
        private bool RemoveFromShopInventory(Order order) {
            // check if inventory contains all ingredients in order, return false early if any not found
            if (!shopManagerData.shopInventory.Contains(order.drinkBase)) return false;
            if (!shopManagerData.shopInventory.Contains(order.drinkFoam)) return false;
            if (!shopManagerData.shopInventory.Contains(order.drinkSweetener)) return false;
            if (order.drinkToppings.Any(topping => !shopManagerData.shopInventory.Contains(topping))) return false;
            
            // remove ingredients from shopInventory
            shopManagerData.shopInventory[shopManagerData.shopInventory.IndexOf(order.drinkBase)].RemoveFromStack(1);
            shopManagerData.shopInventory[shopManagerData.shopInventory.IndexOf(order.drinkFoam)].RemoveFromStack(1);
            shopManagerData.shopInventory[shopManagerData.shopInventory.IndexOf(order.drinkSweetener)].RemoveFromStack(1);
            foreach (var topping in order.drinkToppings)
                shopManagerData.shopInventory[shopManagerData.shopInventory.IndexOf(topping)].RemoveFromStack(1);
            return true;
        }

        /// <summary>
        /// Gets total "Shop Sell Value" of the given order
        /// </summary>
        /// <param name="order">Order containing ingredients to get value of</param>
        /// <returns>Shop Sell Value of order</returns>
        private int GetValueOfOrder(Order order) {
            int value = 0;
            value += order.drinkBase.resourceShopSellValue;
            value += order.drinkFoam.resourceShopSellValue;
            value += order.drinkSweetener.resourceShopSellValue;
            value += order.drinkToppings.Sum(topping => topping.resourceShopSellValue);
            // shopSelectionScore influence on value
            value = Mathf.CeilToInt(value * (shopSelectionScore / 100f + 1));
            return value;
        }
        
        public void HandleOrderGeneration() {
            Order order = GenerateOrder();
            int value = GetValueOfOrder(order);
            if (RemoveFromShopInventory(order)) {
                Debug.Log($"Order sold successfully for {value}");
                // add profits to profitsForTheDay
                return;
            }
            Debug.Log("Lacking ingredients to fulfill order");
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
