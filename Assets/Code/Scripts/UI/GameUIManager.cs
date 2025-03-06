using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.UI
{
    public class GameUIManager : MonoBehaviour {
        public static GameUIManager Instance { get; private set; }
        
        public InventoryUIManager inventoryUIManager;

        private void Awake() {
            inventoryUIManager = new InventoryUIManager();
        }
        private void Start() {
            
        }

        private void Update() {
            
        }
    }
}
