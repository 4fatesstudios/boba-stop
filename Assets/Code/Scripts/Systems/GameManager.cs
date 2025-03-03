using System;
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
        private static int saveSlot;
        
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
            
            saveSystem = new SaveSystem();
            
            playerDataManager = new PlayerDataManager(playerData);
            playerData = ScriptableObject.CreateInstance<PlayerData>();
            
            worldDataManager = new WorldDataManager(worldData);
            worldData = ScriptableObject.CreateInstance<WorldData>();
            
            dayCycleManager = new DayCycleManager();
            levelManagerHelper = new LevelManagerHelper();
            shopManager = new ShopManager();
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
            saveSlot = slot;
        }

        public void LoadData() {
            saveSystem.SetSaveLoadSlot(saveSlot);
            saveSystem.LoadData(playerData, worldData);
        }

        public void SaveData() {
            saveSystem.SetSaveLoadSlot(saveSlot);
            saveSystem.SaveData(playerData, worldData);
        }
        #endregion
        
        #region Player Data Functions
        public bool SetPlayerName(string playerName) {
            return playerDataManager.SetPlayerName(playerName);
        }

        public string GetPlayerName() {
            return playerDataManager.GetPlayerName();
        }

        public int GetPlayerPearls() {
            return playerDataManager.GetPlayerPearls();
        }

        public void AddPlayerPearls(int pearls) {
            playerDataManager.AddPlayerPearls(pearls);
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