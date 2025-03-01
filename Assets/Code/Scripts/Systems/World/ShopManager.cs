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
        private int maxShopSelectionSize;
        private int shopSelectionScore;
        private int ordersGeneratedPotential;
        private bool shopIsRunning;

        public override void Start() {
            shopSelection = new List<Resource>();
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
