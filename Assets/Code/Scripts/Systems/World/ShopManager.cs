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

        public override void Start() {
            
        }

        public override void Update() {
            //
        }

        public override bool LoadData() {
            return true;
        }
        
        public override void Pause() {
            //
        }
        
        public override void Unpause() {
            //
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
        /// Uniquely adds a given resource to the Shop's Selection
        /// </summary>
        /// <param name="resource"></param>
        /// <returns>true if resource was successfully added to selection, false otherwise</returns>
        public bool AddToShopSelection(Resource resource) {
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
