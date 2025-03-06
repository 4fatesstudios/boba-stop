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
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject); // make persistent across scenes
            }
            else {
                Destroy(gameObject); // delete duplicates
            }
            
            inventoryUIManager = new InventoryUIManager();
        }
        private void Start() {
            
        }

        private void Update() {
            
        }
    }
}
