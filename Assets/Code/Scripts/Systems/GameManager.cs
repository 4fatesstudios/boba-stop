using System;
using System.Collections.Generic;
using BobaStop.Data.Level;
using BobaStop.Items;
using BobaStop.Systems.World;
using BobaStop.Data.Saved;
using UnityEngine;

namespace BobaStop.Systems {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }
        
        #region Manager Instances & Related Vars
        private static SaveSystem saveSystem;
        
        public static PlayerDataManager playerDataManager;
        private static PlayerData playerData;
        
        public static WorldDataManager worldDataManager;
        private static WorldData worldData;
        
        private static DayCycleManager dayCycleManager;
        private static LevelManagerHelper levelManagerHelper;
        private static ShopManager shopManager;
        #endregion
        
        #region Unity Functions
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject); // make persistent across scenes
            }
            else {
                Destroy(gameObject); // delete duplicates
            }
            
            saveSystem = new SaveSystem(playerDataManager, worldDataManager);
            
            playerDataManager = new PlayerDataManager(playerData);
            playerData = ScriptableObject.CreateInstance<PlayerData>();
            
            worldDataManager = new WorldDataManager(worldData);
            worldData = ScriptableObject.CreateInstance<WorldData>();
            
            dayCycleManager = new DayCycleManager();
            levelManagerHelper = new LevelManagerHelper();
            shopManager = new ShopManager(worldData);
        }

        private void Start() {
            dayCycleManager.Start();
            shopManager.Start();
        }

        private void Update() {
            dayCycleManager.Update();
        }
        #endregion

        public void PauseTime() {
            dayCycleManager.PauseDay();
        }

        public void UnpauseTime() {
            dayCycleManager.StartDay();
        }
        
        #region SaveSystem Functions
        public void SetSaveSlot(int slot) {
            saveSystem.SetSaveLoadSlot(slot);
        }

        public void GetSaveSlot() {
            saveSystem.GetSaveLoadSlot();
        }

        public void LoadData() {
            saveSystem.LoadData(playerData, worldData);
        }

        public void SaveData() {
            saveSystem.SaveData(playerData, worldData);
        }
        #endregion
        
        #region Day Cycle Functions
        public string GetStandardTime() {
            return dayCycleManager.GetStandardTime();
        }

        public string GetMilitaryTime() {
            return dayCycleManager.GetMilitaryTime();
        }
        
        public void SubscribeToDayPhaseChanges(EventHandler<DayCycleManager.OnDayPhaseChangeEventArgs> handler)
        {
            dayCycleManager.OnDayPhaseChanged += handler;
        }

        public void UnsubscribeFromDayPhaseChanges(EventHandler<DayCycleManager.OnDayPhaseChangeEventArgs> handler)
        {
            dayCycleManager.OnDayPhaseChanged -= handler;
        }
        #endregion

        #region Level Manager Helper
        public void SwitchScene(LevelProperties level) {
            levelManagerHelper.SwitchScene(level);
        }

        #endregion
        
        #region Shop Functions
        public void PrintShopSelection() {
            var shopSelection = shopManager.GetShopSelection();
            foreach (var resource in shopSelection) {
                Debug.Log(resource);
            }
        }

        public List<Resource> ShopManager_GetShopSelection() {
            return worldData.shopSelection;
        }

        public void SetShopSelection(List<Resource> shopSelection) {
            shopManager.SetupShopSelection(shopSelection);
        }

        public bool AddToShopSelection(Resource resource) {
            return shopManager.AddToShopSelection(resource);
        }

        public bool RemoveFromShopSelection(Resource resource) {
            return shopManager.RemoveFromShopSelection(resource);
        }

        public void GenerateOrder() {
            shopManager.GenerateOrder();
        }
        
        #endregion
    }
}