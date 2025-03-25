using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.Systems;
using TMPro;
using UnityEngine;

namespace BobaStop.UI
{
    public class GameUIManager : MonoBehaviour {
        public static GameUIManager Instance { get; private set; }
        
        public InventoryUIManager inventoryUIManager;
        public DialogueUIManager dialogueUIManager;

        [SerializeField] private TextMeshProUGUI time;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject); // make persistent across scenes
            }
            else {
                Destroy(gameObject); // delete duplicates
            }
            
            inventoryUIManager = new InventoryUIManager();
            dialogueUIManager = new DialogueUIManager();
        }
        private void Start() {
            
        }

        private void Update() {
            // time.text = GameManager.Instance.dayCycleManager.GetStandardTime();
        }
    }
}
